Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraEditors.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors.Filtering

Namespace AcurSoft.Data.Filtering

    Public Class NodeComplexElementEx
        Inherits NodeEditableElementEx
        'Public Overrides ReadOnly Property ElementNodeIndex As Integer
        Public Overridable ReadOnly Property ParentheseOpen As NodeParentheseElementEx
        Public Overridable ReadOnly Property ParentheseClose As NodeParentheseElementEx

        Public Overrides ReadOnly Property NeedReprocessing As Boolean
            Get
                Return Me.ProcessedElements.Any(Function(q) q IsNot Me AndAlso q.NeedReprocessing)
            End Get
        End Property

        Private _ProcessedElements As List(Of NodeEditableElementEx)
        Public Overridable ReadOnly Property ProcessedElements As List(Of NodeEditableElementEx)
            Get
                If _ProcessedElements Is Nothing Then
                    _ProcessedElements = Me.CollectElements()
                End If
                Return _ProcessedElements
            End Get
        End Property


        Private _SubElements As List(Of NodeEditableElementEx)
        Public Overridable ReadOnly Property SubElements As List(Of NodeEditableElementEx)
            Get
                If _SubElements Is Nothing Then
                    _SubElements = New List(Of NodeEditableElementEx)
                End If
                Return _SubElements
            End Get
        End Property

        Private _ParametersCriteria As List(Of CriteriaOperator)
        Protected Overridable ReadOnly Property ParametersCriteria As List(Of CriteriaOperator)
            Get
                If _ParametersCriteria Is Nothing Then
                    _ParametersCriteria = New List(Of CriteriaOperator)
                End If
                Return _ParametersCriteria
            End Get
        End Property

        Public Overrides Property Criteria As CriteriaOperator


        Public Sub New(ByVal node As ClauseNodeEx, ByVal elementType As ElementType, ByVal text As String, elementNodeIndex As Integer, criteria As CriteriaOperator)
            MyBase.New(node, elementType, text)
            Me.Criteria = criteria
            Me.ElementNodeIndex = elementNodeIndex
            Me.ParentheseOpen = New NodeParentheseElementEx(Me, NodeParentheseKindEnum.Open) With {.Opposite = New NodeParentheseElementEx(Me, NodeParentheseKindEnum.Close)}
            Me.ParentheseClose = Me.ParentheseOpen.Opposite
            Me.ParentheseClose.Opposite = Me.ParentheseOpen

        End Sub

        'Public Overridable Function ToCriteria() As CriteriaOperator
        '    Return Nothing
        'End Function


        Public Function CanBuildElements() As Boolean
            Return Me.CanBuildElements(Me.GetProcessedParameters())
        End Function

        Protected Function CanBuildElements(lst As List(Of ProcessComplexCriteriaInfos)) As Boolean
            Return Not lst.Any(Function(q) q.NeedProcessing)
        End Function


        'Public Function GetUnProcessedParameters() As List(Of ProcessCriteriaInfos)
        '    Return Me.GetProcessedParameters.Where(Function(q) q.NeedProcessing).ToList
        'End Function


        Public Function GetProcessedParameters() As List(Of ProcessComplexCriteriaInfos)
            Dim lst As New List(Of ProcessComplexCriteriaInfos)
            For i As Integer = 0 To Me.ParametersCriteria.Count - 1
                lst.Add(New ProcessComplexCriteriaInfos(Me, Me.ParametersCriteria(i), i))
            Next
            Return lst
        End Function

        Public Function CanProcessCriteria(cr As CriteriaOperator) As Boolean
            Dim kind As CriteriaKind = cr.GetCritriaKind()
            Select Case kind
                Case CriteriaKind.OperandValue, CriteriaKind.OperandProperty
                    Return True
            End Select
            Return False
        End Function


        Public Overridable Function BuildElements() As List(Of NodeEditableElementEx)
            Return Me.BuildElements(Me.GetProcessedParameters())
        End Function


        Public Overridable Function BuildElements(processCriteriaInfos As List(Of ProcessComplexCriteriaInfos)) As List(Of NodeEditableElementEx)
            Dim lst As New List(Of NodeEditableElementEx)
            If Me.CanBuildElements(processCriteriaInfos) Then
                lst = Me.CollectElements(processCriteriaInfos)
            End If
            Return lst
        End Function

        Public Overridable Function CollectElements(processCriteriaInfos As List(Of ProcessComplexCriteriaInfos)) As List(Of NodeEditableElementEx)
            Dim lst As New List(Of NodeEditableElementEx)
            Return lst
        End Function

        Public Overridable Function CollectElements() As List(Of NodeEditableElementEx)
            Dim lst As New List(Of NodeEditableElementEx)
            Return Me.CollectElements(Me.GetProcessedParameters())
        End Function
    End Class
End Namespace
