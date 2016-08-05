
Imports DevExpress.Data.Filtering

Namespace AcurSoft.Data.Filtering.Helpers


    Public Class ProcessComplexCriteriaInfos
        Public Property NeedProcessing As Boolean
        Public ReadOnly Property Criteria As CriteriaOperator
        Public ReadOnly Property CriteriaKind As CriteriaKind
        Public ReadOnly Property ParameterIndex As Integer
        Public ReadOnly Property MainElement As NodeComplexElementEx

        Private _Elements As List(Of NodeEditableElementEx)
        Public ReadOnly Property Elements As List(Of NodeEditableElementEx)
            Get
                If _Elements Is Nothing Then
                    _Elements = New List(Of NodeEditableElementEx)
                End If
                Return _Elements
            End Get
        End Property

        Public Sub New(elm As NodeComplexElementEx, criteria As CriteriaOperator, parameterIndex As Integer)
            Me.MainElement = elm
            Me.Criteria = criteria
            Me.CriteriaKind = criteria.GetCritriaKind()
            Me.ParameterIndex = parameterIndex
            Me.NeedProcessing = True
            Select Case Me.CriteriaKind
                Case CriteriaKind.OperandValue
                    With New NodeParameterValueElementEx(elm, DirectCast(criteria, OperandValue), parameterIndex)
                        Me.Elements.AddRange(.Elements)
                    End With
                    Me.NeedProcessing = False
                Case CriteriaKind.OperandProperty
                    With New NodeParameterValueElementEx(elm, DirectCast(criteria, OperandProperty), parameterIndex)
                        Me.Elements.AddRange(.Elements)
                    End With
                    Me.NeedProcessing = False
                Case CriteriaKind.FunctionOperator
                    Dim fe As New NodeFunctionElementEx(elm, parameterIndex, DirectCast(criteria, FunctionOperator))
                    Me.BuildComplexElement(fe)
                Case CriteriaKind.BinaryOperator
                    Dim bo As BinaryOperator = DirectCast(criteria, BinaryOperator)
                    If bo.IsMathOperation Then
                        Dim be As New NodeMathElementEx(elm, parameterIndex, bo)
                        Me.BuildComplexElement(be)
                    ElseIf bo.IsEqualityOperation
                        Dim be As New NodeEqualityElementEx(elm, parameterIndex, bo)
                        Me.BuildComplexElement(be)
                    End If

            End Select
        End Sub
        Public Sub BuildComplexElement(complexElement As NodeComplexElementEx)
            Dim elms As List(Of NodeEditableElementEx) = complexElement.ProcessedElements

            While elms.Any(Function(q) q.NeedReprocessing)
                Dim e As NodeEditableElementEx = elms.FirstOrDefault(Function(q) q.NeedReprocessing)
                Dim pos As Integer = elms.IndexOf(e)
                If TypeOf e Is NodeComplexElementEx Then
                    elms.InsertRange(pos + 1, DirectCast(e, NodeComplexElementEx).CollectElements)
                    elms.Remove(e)
                End If
            End While
            Me.Elements.AddRange(elms)
            Me.NeedProcessing = False
        End Sub

    End Class

End Namespace
