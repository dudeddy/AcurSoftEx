Imports System.IO
Imports System.Reflection
Imports AcurSoft.Data.Filtering
Imports AcurSoft.Data.Filtering.Helpers
Imports AcurSoft.XtraEditors
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Filtering
Imports DevExpress.XtraFilterEditor
Imports DevExpress.XtraTab

Namespace AcurSoft.XtraFilterEditor

    Public Class FilterEditorControlEx
        Inherits FilterEditorControl

        Public Enum EditKindEnum
            VisualOnly
            TextOnly
            Visual
            Text
        End Enum

        Private _EditKind As EditKindEnum
        Public Property EditKind As EditKindEnum
            Get
                Return _EditKind
            End Get
            Set(value As EditKindEnum)
                Select Case value
                    Case EditKindEnum.Text
                        Me.ActiveView = FilterEditorActiveView.Text
                    Case EditKindEnum.TextOnly
                        Me.ActiveView = FilterEditorActiveView.Text
                        Me.Tab.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False
                    Case EditKindEnum.VisualOnly
                        Me.ActiveView = FilterEditorActiveView.Visual
                        Me.Tab.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False
                    Case Else
                        Me.ActiveView = FilterEditorActiveView.Visual
                End Select
                _EditKind = value
            End Set
        End Property
        Public ReadOnly Property MainTabControl As XtraTabControl
            Get
                Return Me.Tab
            End Get
        End Property

        Public ReadOnly Property TabVisual As XtraTabPage
        Public ReadOnly Property TabText As XtraTabPage


        Public Sub New()
            MyBase.New()
            Me.AllowAggregateEditing = DevExpress.XtraEditors.FilterControlAllowAggregateEditing.AggregateWithCondition
            Me.ShowGroupCommandsIcon = True
            Me.ShowIsNullOperatorsForStrings = True
            Me.ShowOperandTypeIcon = True
            Me.UseMenuForOperandsAndOperators = True
            Me.Tab.HeaderLocation = TabHeaderLocation.Right
            Me.EditKind = EditKindEnum.Visual
            _TabVisual = Me.TreePage
            _TabVisual.Text = ""
            _TabVisual.Tooltip = "Visual Filter Editor"
            _TabText = Me.Tab.TabPages.FirstOrDefault(Function(q) q IsNot Me.TreePage)
            _TabText.Text = ""
            _TabText.Tooltip = "Advanced Filter Editor"

            _TabVisual.Image = Ressources.filter_visual_16x16
            _TabText.Image = Ressources.filter_text_16x16

        End Sub

        Protected Overrides Function CreateTreeControl() As FilterControl
            'Return MyBase.CreateTreeControl()
            Return New FilterControlEx
        End Function

        Public ReadOnly Property HasSkippedCriterias() As Boolean
        Public Function CanBeDisplayedByTreeEx() As Boolean
            Dim cr As CriteriaOperator = CriteriaOperator.TryParse(Me.FilterString)
            If cr Is Nothing Then Return False
            Return True
            Dim skippedCriteriaOperator As New List(Of CriteriaOperator)()
            CriteriaToTreeProcessorEx.GetTree(New FilterControlNodesFactory(Tree.Model), cr, skippedCriteriaOperator)
            _HasSkippedCriterias = (skippedCriteriaOperator.Count <> 0)
            Return Not _HasSkippedCriterias
        End Function

        'Protected Overrides Function CanBeDisplayedByTree(criteria As CriteriaOperator) As Boolean
        '    'Return MyBase.CanBeDisplayedByTree(criteria)
        '    'Return True
        '    Dim skippedCriteriaOperator As New List(Of CriteriaOperator)()
        '    CriteriaToTreeProcessorEx.GetTree(New FilterControlNodesFactory(Tree.Model), criteria, skippedCriteriaOperator)
        '    _HasSkippedCriterias = (skippedCriteriaOperator.Count <> 0)
        '    Return Not _HasSkippedCriterias
        'End Function

        'Public Function HasSkippedCriterias() As Boolean
        '    Dim skippedCriteriaOperator As New List(Of CriteriaOperator)()
        '    CriteriaToTreeProcessorEx.GetTree(New FilterControlNodesFactory(Tree.Model), Me.FilterCriteria, skippedCriteriaOperator)
        '    Return skippedCriteriaOperator.Count = 0
        'End Function

    End Class
End Namespace
