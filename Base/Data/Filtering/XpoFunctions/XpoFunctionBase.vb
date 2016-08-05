Imports System.Reflection
Imports DevExpress.Data.Filtering
Imports System.Linq
Imports DevExpress.Xpo
Imports DevExpress.Xpo.Helpers
Imports System.Linq.Expressions
Imports DevExpress.Data.Linq

Namespace AcurSoft.Data.Filtering

    Public Class XpoFunctionBase
        Implements ICustomFunctionOperatorBrowsable, ICustomFunctionOperatorQueryable, ICustomCriteriaOperatorQueryable ',
        '  ICustomFunctionOperatorConvertibleToExpression ', ICustomFunctionOperatorFormattable

        Public Property Id As Integer
        Public ReadOnly Property MethodInfo As MethodInfo
        Public ReadOnly Property MethodName As String
        Public ReadOnly Property ArgsCount As Integer
        Public ReadOnly Property ArgsInfos As ParameterInfo()
        Public ReadOnly Property TargetObject As Object
        Public ReadOnly Property ParamsFixers As Func(Of Object(), Object())
        Public ReadOnly Property Evaluator As Func(Of Object(), Object)
        Public ReadOnly Property ParamsTypes As List(Of Type)
        Public ReadOnly Property DefaultDescription As String
        Public ReadOnly Property Caption As String
        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(mi As MethodInfo)
            Me.New(mi, mi.Name)
        End Sub
        Public ReadOnly Property EvaluatorDelegate As [Delegate]
        Public Sub New(exp As LambdaExpression, functionName As String)
            _MethodName = functionName
            _EvaluatorDelegate = exp.Compile()
            Me.Init(_EvaluatorDelegate.Method)
            Me.Evaluator = Function(operands() As Object) _EvaluatorDelegate.DynamicInvoke(Me.ParamsFixers(operands))
        End Sub

        Public Sub New(evaluatorDelegate As [Delegate], functionName As String)
            _MethodName = functionName
            _EvaluatorDelegate = evaluatorDelegate
            Me.Init(_EvaluatorDelegate.Method)
            Me.Evaluator = Function(operands() As Object) _EvaluatorDelegate.DynamicInvoke(Me.ParamsFixers(operands))
        End Sub

        Public Sub Init(mi As MethodInfo)
            _MethodInfo = mi
            _ArgsInfos = mi.GetParameters()
            _ArgsCount = _ArgsInfos.Count
            _ParamsTypes = _ArgsInfos.Select(Function(q) q.ParameterType).ToList()
            _MinOperandCount = System.Convert.ToInt32(_ArgsInfos.LongCount(Function(q) Not q.IsOptional))

            _Caption = String.Format("{0}({1})", _MethodName, String.Join(", ", Me.ArgsInfos.Select(Function(q) If(q.IsOptional, "[Optional] ", "") & q.Name & " As " & q.ParameterType.Name)))

            _DefaultDescription = "Function: " & _Caption & ":" & Environment.NewLine
            If _ArgsCount = 0 Then
                _DefaultDescription &= ""
            Else
                Dim desc As New List(Of String)
                For x As Integer = 0 To _ArgsCount - 1
                    desc.Add(String.Format("Argument {0}: {1} Type must be : {2}{3}", x + 1, Me.ArgsInfos(x).Name, Me.ArgsInfos(x).ParameterType.Name, If(Me.ArgsInfos(x).IsOptional, " [Optional]", "")))
                Next
                _DefaultDescription &= String.Join(Environment.NewLine & ",", desc)
            End If

            _ParamsFixers = Function(q As Object())
                                If _ArgsCount = 0 Then Return New Object() {}
                                Dim newArgs As New List(Of Object)
                                'Dim x As Integer = 0
                                For x As Integer = 0 To q.Length - 1
                                    Dim a As ParameterInfo = _ArgsInfos(x)
                                    If TypeOf q(x) Is IConvertible AndAlso q(x).GetType IsNot a.ParameterType Then
                                        newArgs.Add(System.Convert.ChangeType(q(x), a.ParameterType))
                                    Else
                                        newArgs.Add(q(x))
                                    End If
                                Next
                                Return newArgs.ToArray()
                            End Function
        End Sub


        Public Sub New(mi As MethodInfo, functionName As String)
            _MethodName = functionName
            Me.Init(mi)
            If Not mi.IsStatic Then
                If MethodInfo.DeclaringType Is GetType(XpoFunctionsHelper) Then
                    _TargetObject = XpoFunctionsHelper.Instance
                Else
                    _TargetObject = Activator.CreateInstance(_MethodInfo.DeclaringType)
                End If
            End If
            Me.Evaluator = Function(operands() As Object) _MethodInfo.Invoke(_TargetObject, Me.ParamsFixers(operands))
        End Sub

#Region "ICustomFunctionOperator Members"
        Public Function Evaluate(ParamArray ByVal operands() As Object) As Object Implements ICustomFunctionOperator.Evaluate
            Return Me.Evaluator.Invoke(operands)
        End Function

        Public ReadOnly Property Name() As String Implements ICustomFunctionOperator.Name
            Get
                Return _MethodName
            End Get
        End Property

        Private _MinOperandCount As Integer
        Public ReadOnly Property MinOperandCount As Integer Implements ICustomFunctionOperatorBrowsable.MinOperandCount
            Get
                Return _MinOperandCount
            End Get
        End Property

        Public ReadOnly Property MaxOperandCount As Integer Implements ICustomFunctionOperatorBrowsable.MaxOperandCount
            Get
                Return _ArgsCount
            End Get
        End Property

        Public ReadOnly Property Description As String Implements ICustomFunctionOperatorBrowsable.Description
            Get
                Return Me.DefaultDescription
            End Get
        End Property

        Public ReadOnly Property Category As FunctionCategory Implements ICustomFunctionOperatorBrowsable.Category
            Get
                Return FunctionCategory.All
            End Get
        End Property

        Public Overridable Function ResultType(ParamArray ByVal operands() As Type) As Type Implements ICustomFunctionOperator.ResultType
            Return _MethodInfo.ReturnType
        End Function

        Public Function IsValidOperandCount(count As Integer) As Boolean Implements ICustomFunctionOperatorBrowsable.IsValidOperandCount
            Return count >= _MinOperandCount AndAlso count <= _ArgsCount
        End Function

        Public Function IsValidOperandType(operandIndex As Integer, operandCount As Integer, type As Type) As Boolean Implements ICustomFunctionOperatorBrowsable.IsValidOperandType
            If operandCount = 0 Then Return True
            Return type.IsCastableTo(_ParamsTypes(operandIndex))
        End Function

        Private Function GetMethodInfo() As MethodInfo Implements ICustomFunctionOperatorQueryable.GetMethodInfo, ICustomCriteriaOperatorQueryable.GetMethodInfo
            Return _MethodInfo
        End Function

        Public Function GetCriteria(ParamArray operands() As CriteriaOperator) As CriteriaOperator Implements ICustomCriteriaOperatorQueryable.GetCriteria
            If operands Is Nothing OrElse operands.Length <> 1 OrElse Not (TypeOf operands(0) Is MemberInitOperator) Then
            End If
            Return New FunctionOperator(_MethodName, CType(operands(0), MemberInitOperator).Members.Select(Function(q) q.Property))
        End Function

        'Public Function Convert(converter As ICriteriaToExpressionConverter, ParamArray operands() As Expression) As Expression Implements ICustomFunctionOperatorConvertibleToExpression.Convert
        '    Throw New NotImplementedException()
        '    Dim f As Func(Of Object) = Function() Me.Evaluator.Invoke(operands)
        '    Return f
        'End Function

#End Region
        Private _DefaultParametersCriteria As List(Of CriteriaOperator)
        Public Overridable Property DefaultParametersCriteria As List(Of CriteriaOperator)
            Get
                If _DefaultParametersCriteria Is Nothing Then
                    _DefaultParametersCriteria = New List(Of CriteriaOperator)
                End If
                Return _DefaultParametersCriteria
            End Get
            Set(value As List(Of CriteriaOperator))
                _DefaultParametersCriteria = value
            End Set
        End Property

        Public Overridable Function GetInfo() As CriteriaFunctionBaseInfo
            Return New CriteriaFunctionBaseInfo(Me)
        End Function

    End Class

End Namespace


