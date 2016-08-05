Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Class NodeMathElementEx
        Inherits NodeComplexElementEx
        Implements INodeConvertibleToCriteria

        Public Enum MathOperationKind
            Divide = BinaryOperatorType.Divide
            Modulo = BinaryOperatorType.Modulo
            Multiply = BinaryOperatorType.Multiply
            Plus = BinaryOperatorType.Plus
            Minus = BinaryOperatorType.Minus
        End Enum
        'Public Overridable ReadOnly Property Parent As NodeEditableElementEx
        Public Overrides ReadOnly Property NodeKind As NodeElementKindEnum
        Public Overridable ReadOnly Property BinaryOperator As BinaryOperator
        Public ReadOnly Property OperationKind As MathOperationKind

        Private _LeftElements As List(Of NodeEditableElementEx)
        Public Overridable ReadOnly Property LeftElements As List(Of NodeEditableElementEx)
            Get
                If _LeftElements Is Nothing Then
                    _LeftElements = New List(Of NodeEditableElementEx)
                End If
                Return _LeftElements
            End Get
        End Property


        Private _RightElements As List(Of NodeEditableElementEx)
        Public Overridable ReadOnly Property RightElements As List(Of NodeEditableElementEx)
            Get
                If _RightElements Is Nothing Then
                    _RightElements = New List(Of NodeEditableElementEx)
                End If
                Return _RightElements
            End Get
        End Property


        Public Sub New(ByVal elm As NodeComplexElementEx, parameterIndex As Integer, binaryOperator As BinaryOperator)
            Me.New(elm.NodeEx, elm.ElementNodeIndex, binaryOperator)
            Me.ParameterIndex = parameterIndex
            Me.ParameterOf = elm
        End Sub

        Public Sub New(ByVal node As ClauseNodeEx, elementNodeIndex As Integer, binaryOperator As BinaryOperator)
            MyBase.New(node, ElementType.Operation, "###", elementNodeIndex, binaryOperator)
            Me.BinaryOperator = binaryOperator
            Me.NodeKind = NodeElementKindEnum.Math
            Me.OperationKind = DirectCast(binaryOperator.OperatorType, MathOperationKind)
            'Me.SetText(Me.GetNodeText(Me.OperationKind))
            Me.Text = Me.GetNodeText(Me.OperationKind)
            Me.ParametersCriteria.Add(binaryOperator.LeftOperand)
            Me.ParametersCriteria.Add(binaryOperator.RightOperand)

        End Sub

        Public Function GetNodeText(mathOperationKind As MathOperationKind) As String
            Select Case mathOperationKind
                Case MathOperationKind.Divide
                    Return " / "
                Case MathOperationKind.Modulo
                    Return " % "
                Case MathOperationKind.Multiply
                    Return " * "
                Case MathOperationKind.Plus
                    Return " + "
                Case MathOperationKind.Minus
                    Return " - "
                Case Else
                    Return ""
            End Select
        End Function


        Public Overrides Function CollectElements(processCriteriaInfos As List(Of ProcessComplexCriteriaInfos)) As List(Of NodeEditableElementEx)
            Dim lst As New List(Of NodeEditableElementEx)

            Me.SubElements.Clear()
            Me.SubElements.Add(Me.ParentheseOpen)
            'Dim cnt As Integer = 2 - 1
            For i As Integer = 0 To 1
                Me.SubElements.AddRange(processCriteriaInfos(i).Elements)
                If i = 0 Then
                    Me.SubElements.Add(Me)
                End If
            Next
            Me.SubElements.Add(Me.ParentheseClose)
            lst.AddRange(Me.SubElements)
            Return lst
        End Function

        Public Function INodeConvertibleToCriteria_ToCriteria() As CriteriaOperator Implements INodeConvertibleToCriteria.ToCriteria
            Dim bo As New BinaryOperator() With {.OperatorType = Me.BinaryOperator.OperatorType}
            Dim criterias As IEnumerable(Of CriteriaOperator) =
                From q In Me.SubElements
                Where q IsNot Me AndAlso q.ParameterOf Is Me AndAlso TypeOf q Is INodeConvertibleToCriteria
                Select DirectCast(q, INodeConvertibleToCriteria).ToCriteria()
            With criterias
                bo.LeftOperand = .FirstOrDefault
                bo.RightOperand = .LastOrDefault
            End With
            Return bo
        End Function
    End Class
End Namespace
