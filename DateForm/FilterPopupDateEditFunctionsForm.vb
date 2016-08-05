Imports AcurSoft.Data.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid

Public Class FilterPopupDateEditFunctionsForm
    Inherits PopupBaseForm
    Public Property FunctionOperatorType As FunctionOperatorType
    Private _FunctionsInfos As List(Of DateCriteriaFunctionInfo)
    Public ReadOnly Property FunctionsInfos As List(Of DateCriteriaFunctionInfo)
        Get
            If _FunctionsInfos Is Nothing Then
                _FunctionsInfos =
                    (From q In [Enum].GetValues(GetType(FunctionOperatorType)).OfType(Of FunctionOperatorType)
                     Where q.ToString().StartsWith("IsOutlookInterval")
                     Select New DateCriteriaFunctionInfo With {
                              .FunctionOperatorType = q,
                              .Criteria = New FunctionOperator(q, New OperandProperty(Me.DateEdit.Properties.CriteriaPropertyName)),
                              .Caption = Localizer.Active.GetLocalizedString(DirectCast([Enum].Parse(GetType(StringId), "FilterCriteriaToStringFunction" & q.ToString()), StringId))
                              }).ToList()

            End If
            Return _FunctionsInfos
        End Get
    End Property

    Private _Criteria As CriteriaOperator
    Public Property Criteria As CriteriaOperator
        Get
            Return _Criteria

        End Get
        Set(value As CriteriaOperator)
            Me.SetCriteria(value)
        End Set
    End Property


    Public Overridable Sub SetCriteria(criteria As CriteriaOperator)
        If criteria Is Nothing Then

        ElseIf TypeOf criteria Is FunctionOperator
            Dim cr As FunctionOperator = DirectCast(criteria, FunctionOperator)
            If cr.OperatorType.ToString().StartsWith("IsOutlookInterval") AndAlso cr.Operands.Count = 1 AndAlso TypeOf cr.Operands(0) Is OperandProperty AndAlso DirectCast(cr.Operands(0), OperandProperty).PropertyName = DirectCast(Me.OwnerEdit, DateCriteriaEdit).Properties.CriteriaPropertyName Then
                'Me.EditMode = EditModeEnum.Function
                Me.FunctionOperatorType = cr.OperatorType
                _Criteria = cr
            End If


        ElseIf TypeOf criteria Is GroupOperator
            Dim cr As GroupOperator = DirectCast(criteria, GroupOperator)
        End If

        'Me.CalendarControl1.Update()
        'If Me.CalendarControl2 IsNot Nothing Then
        '    Me.CalendarControl2.Update()
        'End If

        'If Me.EditMode = EditModeEnum.None Then
        '    Me.EditMode = EditModeEnum.Normal
        'End If

    End Sub

    Public ReadOnly Property Grid As GridControl
    Public ReadOnly Property DateEdit As DateCriteriaEdit
    Public Sub New(dateEdit As DateCriteriaEdit)
        MyBase.New(dateEdit)
        Me.DateEdit = dateEdit
        Me.Grid = New GridControl
        Me.Controls.Add(Me.Grid)
        Me.Grid.Dock = DockStyle.Top
        Me.Size = Me.CalcFormSizeCore()
        Me.Grid.Height = Me.Height - 26
        Me.Grid.DataSource = Me.FunctionsInfos
        Me.Grid.ForceInitialize()
        With DirectCast(Me.Grid.MainView, GridView)
            .OptionsView.ShowGroupPanel = False
            .OptionsBehavior.Editable = False
            .Columns("FunctionOperatorType").Visible = False
            .Columns("Criteria").Visible = False
            AddHandler .RowCellClick, AddressOf GridRowCellClick
        End With
    End Sub

    Private Sub GridRowCellClick(sender As Object, e As RowCellClickEventArgs)
        If e.Button = MouseButtons.Right Then Return
        'If e.Clicks < 2 Then Return
        Dim fi As DateCriteriaFunctionInfo = DirectCast(Me.Grid.MainView.GetRow(e.RowHandle), DateCriteriaFunctionInfo)
        If fi IsNot Nothing Then
            Me.Criteria = fi.Criteria
            Me.DateEdit.Criteria = fi.Criteria
            MyBase.ClosePopup(PopupCloseMode.Normal)

        End If

    End Sub

    Public Overrides ReadOnly Property ResultValue As Object
        Get
            Return Nothing
        End Get
    End Property

    Protected Overrides Function CalcFormSizeCore() As Size
        Return New Size(450, 350)
    End Function
End Class
