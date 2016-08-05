Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering
Imports System.Linq
Imports DevExpress.XtraEditors.Repository

Namespace AcurSoft.Data.Filtering

    Public Class NodeFunctionElementEx
        Inherits NodeComplexElementEx
        Implements INodeConvertibleToCriteria

        Public Overridable ReadOnly Property FunctionOperator As FunctionOperator
        Public Overrides ReadOnly Property NodeKind As NodeElementKindEnum
        Public ReadOnly Property CustomFunction As ICustomFunctionOperator
        Public ReadOnly Property XpoFunction As XpoFunctionBase
        Public ReadOnly Property XpoFunctionFilterInfo As XpoFunctionFilterInfo

        Public Sub New(ByVal elm As NodeComplexElementEx, parameterIndex As Integer, functionOperator As FunctionOperator)
            Me.New(elm.NodeEx, elm.ElementNodeIndex, functionOperator)
            Me.ParameterIndex = parameterIndex
            Me.ParameterOf = elm
        End Sub

        Public Sub New(ByVal node As ClauseNodeEx, elementNodeIndex As Integer, functionOperator As FunctionOperator)
            MyBase.New(node, ElementType.Operation, functionOperator.Operands(0).ToString().Trim("'"c), elementNodeIndex, functionOperator)
            Me.NodeKind = NodeElementKindEnum.Function
            Me.FunctionOperator = functionOperator
            Me.ParametersCriteria.AddRange(functionOperator.Operands.Skip(1))

            Dim str As String = Me.Text.Trim("'"c)
            If Not str.IsNumeric() AndAlso Not str.StartsWith("@") AndAlso Not str.StartsWith("#") AndAlso Not {"?", "     "}.Contains(str) AndAlso Not (str.Contains("[") OrElse str.Contains("]")) AndAlso Not Me.NodeEx.ModelEx.GetBoundProperty(str).HasValue Then
                Dim fncName As String = str.Split("("c).FirstOrDefault
                _CustomFunction = CriteriaOperator.GetCustomFunction(fncName)
                If _CustomFunction IsNot Nothing Then
                    'Me.SetText(_CustomFunction.Name)
                    Me.Text = _CustomFunction.Name
                    If TypeOf _CustomFunction Is XpoFunctionBase Then
                        _XpoFunction = DirectCast(_CustomFunction, XpoFunctionBase)
                        _XpoFunctionFilterInfo = XpoFunctionFilterManager.GetInfos(_CustomFunction.Name)
                    End If
                End If

            End If

        End Sub

        Public Overrides Function CollectElements(processCriteriaInfos As List(Of ProcessComplexCriteriaInfos)) As List(Of NodeEditableElementEx)
            Dim lst As New List(Of NodeEditableElementEx)

            Me.SubElements.Clear()
            Me.SubElements.Add(Me.ParentheseOpen)
            Dim cnt As Integer = processCriteriaInfos.Count - 1
            For i As Integer = 0 To cnt
                Me.SubElements.AddRange(processCriteriaInfos(i).Elements)
                If i <> cnt Then
                    Me.SubElements.Add(New NodeCommaElementEx(Me.NodeEx))
                End If
            Next
            Me.SubElements.Add(Me.ParentheseClose)
            lst.Add(Me)
            lst.AddRange(Me.SubElements)
            Return lst
        End Function

        Public Function INodeConvertibleToCriteria_ToCriteria() As CriteriaOperator Implements INodeConvertibleToCriteria.ToCriteria
            '    Throw New NotImplementedException()
            Dim fe As New FunctionOperator(Me.FunctionOperator.OperatorType, Me.FunctionOperator.Operands(0))
            Dim criterias As IEnumerable(Of CriteriaOperator) =
                From q In Me.SubElements
                Where q IsNot Me AndAlso q.ParameterOf Is Me AndAlso TypeOf q Is INodeConvertibleToCriteria
                Select DirectCast(q, INodeConvertibleToCriteria).ToCriteria()

            fe.Operands.AddRange(criterias)
            Return fe
        End Function

    End Class
End Namespace
