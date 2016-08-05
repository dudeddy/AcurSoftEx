Imports System.ComponentModel
Imports DevExpress.Data.Filtering
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Calendar
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Helpers
Imports DevExpress.XtraEditors.Popup

Public Class FilterPopupDateEditForm
    Inherits PopupDateEditForm
    Public Event OnResult(isCancel As Boolean)
    Public Event OnIsRangeCheckEditCheckedChanged(checkState As CheckState)

    Public Property FunctionOperatorType As FunctionOperatorType

    Private _EditMode As EditModeEnum ' = EditModeEnum.None
    Public Property EditMode As EditModeEnum
        Get
            Return _EditMode
        End Get
        Set(value As EditModeEnum)
            If value = _EditMode Then Return
            Select Case value
                Case EditModeEnum.Normal
                    Me.IsRange = False
                    Me.IsOneDate = False
                Case EditModeEnum.Lesser, EditModeEnum.Greater, EditModeEnum.LessOrEqual, EditModeEnum.GreaterOrEqual
                    Me.IsRange = False
                    Me.IsOneDate = True
                Case EditModeEnum.Between
                    Me.IsRange = True
            End Select
            _EditMode = value
        End Set
    End Property

    'Private _Criteria As CriteriaOperator
    Public Property Criteria As CriteriaOperator
        Get
            Return Me.GetCriteria

        End Get
        Set(value As CriteriaOperator)
            Me.SetCriteria(value)
        End Set
    End Property

    Protected Overrides Function CalcCalendarInitialDate() As Date
        Dim cr_org As CriteriaOperator = Me.Criteria
        If cr_org Is Nothing Then
            Return MyBase.CalcCalendarInitialDate()
        ElseIf TypeOf cr_org Is BetweenOperator Then
            Dim cr As BetweenOperator = DirectCast(cr_org, BetweenOperator)
            If TypeOf cr.BeginExpression Is OperandValue AndAlso TypeOf DirectCast(cr.BeginExpression, OperandValue).Value Is Date AndAlso TypeOf cr.EndExpression Is OperandValue AndAlso TypeOf DirectCast(cr.EndExpression, OperandValue).Value Is Date Then
                Return DirectCast(DirectCast(cr.BeginExpression, OperandValue).Value, Date)
            End If
        ElseIf TypeOf cr_org Is BinaryOperator
            Dim cr As BinaryOperator = DirectCast(cr_org, BinaryOperator)
            If TypeOf cr.RightOperand Is OperandValue AndAlso TypeOf DirectCast(cr.RightOperand, OperandValue).Value Is Date Then
                Select Case cr.OperatorType
                    Case BinaryOperatorType.Greater, BinaryOperatorType.GreaterOrEqual, BinaryOperatorType.Less, BinaryOperatorType.LessOrEqual, BinaryOperatorType.Equal
                        Return DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                End Select
            End If
        End If
        Return MyBase.CalcCalendarInitialDate()
    End Function

    Public Overridable Sub SetCriteria(criteria As CriteriaOperator)
        If criteria Is Nothing Then

        ElseIf TypeOf criteria Is BetweenOperator Then
            Dim cr As BetweenOperator = DirectCast(criteria, BetweenOperator)
            If TypeOf cr.BeginExpression Is OperandValue AndAlso TypeOf DirectCast(cr.BeginExpression, OperandValue).Value Is Date AndAlso TypeOf cr.EndExpression Is OperandValue AndAlso TypeOf DirectCast(cr.EndExpression, OperandValue).Value Is Date Then
                Me.EditMode = EditModeEnum.Between
                Me.CalendarControl1.DateTime = DirectCast(DirectCast(cr.BeginExpression, OperandValue).Value, Date)
                'Me.Calendar.SelectedRanges.Clear()
                'Dim d As Date = DirectCast(DirectCast(cr.BeginExpression, OperandValue).Value, Date)
                'Me.DateTime = d
                'Me.Calendar.SelectedRanges.AddRange({New DateRange(d, d.AddDays(1))})


                Me.CalendarControl2.DateTime = DirectCast(DirectCast(cr.EndExpression, OperandValue).Value, Date)
            End If
        ElseIf TypeOf criteria Is BinaryOperator
            Dim cr As BinaryOperator = DirectCast(criteria, BinaryOperator)
            If TypeOf cr.RightOperand Is OperandValue AndAlso TypeOf DirectCast(cr.RightOperand, OperandValue).Value Is Date Then
                Select Case cr.OperatorType
                    Case BinaryOperatorType.Greater
                        Me.EditMode = EditModeEnum.Greater
                        Me.Calendar.DateTime = DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                    Case BinaryOperatorType.GreaterOrEqual
                        Me.EditMode = EditModeEnum.GreaterOrEqual
                        Me.Calendar.DateTime = DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                    Case BinaryOperatorType.Less
                        Me.EditMode = EditModeEnum.Lesser
                        Me.Calendar.DateTime = DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                    Case BinaryOperatorType.LessOrEqual
                        Me.EditMode = EditModeEnum.LessOrEqual
                        Me.Calendar.DateTime = DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                    Case BinaryOperatorType.Equal
                        Me.EditMode = EditModeEnum.Normal
                        Me.Calendar.SelectedRanges.Clear()
                        Dim d As Date = DirectCast(DirectCast(cr.RightOperand, OperandValue).Value, Date)
                        Me.Calendar.SelectedRanges.AddRange({New DateRange(d, d.AddDays(1))})
                End Select
            End If
        ElseIf TypeOf criteria Is FunctionOperator
            Dim cr As FunctionOperator = DirectCast(criteria, FunctionOperator)
            If cr.OperatorType.ToString().ToString() = "IsOutlookInterval" AndAlso cr.Operands.Count = 1 AndAlso TypeOf cr.Operands(0) Is OperandProperty AndAlso DirectCast(cr.Operands(0), OperandProperty).PropertyName = DirectCast(Me.OwnerEdit, DateCriteriaEdit).Properties.CriteriaPropertyName Then
                Me.EditMode = EditModeEnum.Function
                Me.FunctionOperatorType = cr.OperatorType
            End If


        ElseIf TypeOf criteria Is GroupOperator
            Dim cr As GroupOperator = DirectCast(criteria, GroupOperator)
            If cr.Operands.Count Mod 2 = 1 Then Return
            Dim dateRangeCollection As New DateRangeCollection
            Dim rangeIsOk As Boolean = False
            For i As Integer = 0 To cr.Operands.Count - 1 Step 2
                If TypeOf cr.Operands(i) Is BinaryOperator AndAlso DirectCast(cr.Operands(i), BinaryOperator).OperatorType = BinaryOperatorType.GreaterOrEqual AndAlso TypeOf cr.Operands(i + 1) Is BinaryOperator AndAlso DirectCast(cr.Operands(i + 1), BinaryOperator).OperatorType = BinaryOperatorType.Less Then
                    Dim bo1 As BinaryOperator = DirectCast(cr.Operands(i), BinaryOperator)
                    Dim bo2 As BinaryOperator = DirectCast(cr.Operands(i + 1), BinaryOperator)
                    If TypeOf bo1.RightOperand Is OperandValue AndAlso TypeOf DirectCast(bo1.RightOperand, OperandValue).Value Is Date AndAlso TypeOf bo2.RightOperand Is OperandValue AndAlso TypeOf DirectCast(bo2.RightOperand, OperandValue).Value Is Date Then
                        dateRangeCollection.Add(New DateRange(DirectCast(DirectCast(bo1.RightOperand, OperandValue).Value, Date), DirectCast(DirectCast(bo2.RightOperand, OperandValue).Value, Date)))
                        rangeIsOk = True
                    End If
                End If
            Next
            If rangeIsOk Then
                Me.EditMode = EditModeEnum.Normal
                Me.Calendar.SelectedRanges.Clear()
                Me.Calendar.SelectedRanges.AddRange(dateRangeCollection)
            End If
        End If

        'Me.CalendarControl1.Update()
        'If Me.CalendarControl2 IsNot Nothing Then
        '    Me.CalendarControl2.Update()
        'End If

        If Me.EditMode = EditModeEnum.None Then
            Me.EditMode = EditModeEnum.Normal
        End If

    End Sub


    Public Overridable Function GetCriteria() As CriteriaOperator
        Dim editor As DateCriteriaEdit = DirectCast(Me.OwnerEdit, DateCriteriaEdit)
        Return Me.GetCriteria(editor.Properties.CriteriaPropertyName)
    End Function

    Public Overridable Function GetCriteria(columnFieldName As String) As CriteriaOperator
        Select Case EditMode
            Case EditModeEnum.Between
                Return New BetweenOperator(columnFieldName, Me.CalendarControl1.DateTime, Me.CalendarControl2.DateTime)
            Case EditModeEnum.Greater
                Return New BinaryOperator(columnFieldName, Me.CalendarControl1.DateTime, BinaryOperatorType.Greater)
            Case EditModeEnum.GreaterOrEqual
                Return New BinaryOperator(columnFieldName, Me.CalendarControl1.DateTime, BinaryOperatorType.GreaterOrEqual)
            Case EditModeEnum.Lesser
                Return New BinaryOperator(columnFieldName, Me.CalendarControl1.DateTime, BinaryOperatorType.Less)
            Case EditModeEnum.LessOrEqual
                Return New BinaryOperator(columnFieldName, Me.CalendarControl1.DateTime, BinaryOperatorType.LessOrEqual)
            Case Else ' EditModeEnum.Normal
                Dim cr As CriteriaOperator = CriteriaOperator.Parse("")
                For Each r In Me.Calendar.SelectedRanges
                    cr = cr And GroupOperator.And(New BinaryOperator(columnFieldName, r.StartDate, BinaryOperatorType.GreaterOrEqual), New BinaryOperator(columnFieldName, r.EndDate, BinaryOperatorType.Less))
                Next
                Return cr
        End Select
    End Function


    Private _IsRange As Boolean
    Public Property IsRange As Boolean
        Get
            Return _IsRange
        End Get
        Set(value As Boolean)
            'If value = _IsRange Then Return
            If _IsRangeChanging Then Return
            _IsRangeChanging = True
            If value Then
                Me.InitRangeForm()
            Else
                Me.Size = _OriginalSize
            End If
            Me.IsRangeCheckEdit.Checked = value
            Me.FixButtonsLocations()
            Me.CancelButton.Visible = value

            _IsRange = value
            _IsRangeChanging = False
        End Set
    End Property

    Private _IsOneDate As Boolean
    Public Property IsOneDate As Boolean
        Get
            Return _IsOneDate
        End Get
        Set(value As Boolean)
            'If value = _IsOneDate Then Return
            If value Then
                Me.Calendar.SelectionMode = Repository.CalendarSelectionMode.Single

            Else
                Me.Calendar.SelectionMode = Repository.CalendarSelectionMode.Multiple
            End If
            Me.OwnerEdit.Properties.SelectionMode = Me.Calendar.SelectionMode
            _IsOneDate = value
        End Set
    End Property
    Public ReadOnly Property CalendarControl1 As DateControlEx
        Get
            Return DirectCast(Me.Calendar, DateControlEx)
        End Get
    End Property

    Public ReadOnly Property CalendarControl2 As DateControlEx

    Protected Overrides Function CreateCalendar() As CalendarControl
        'Return MyBase.CreateCalendar()
        Return New DateControlEx
    End Function

    Public ReadOnly Property IsRangeCheckEdit As CheckEdit
    Public ReadOnly Property OkButton As SimpleButton
    Public Shadows ReadOnly Property CancelButton As SimpleButton

    Private _OriginalSize As Size = Size.Empty

    Private _RangeFormSize As Size = Size.Empty
    Public ReadOnly Property RangeFormSize As Size
        Get
            If _RangeFormSize = Size.Empty Then
                _RangeFormSize = New Size(_OriginalSize.Width * 2, _OriginalSize.Height)
            End If
            Return _RangeFormSize
        End Get
    End Property

    Private _RangeFormInited As Boolean
    Protected Overridable Sub InitRangeForm()
        Me.IsOneDate = True
        If Not _RangeFormInited Then
            _CalendarControl2 = New DateControlEx With {.SelectionMode = Repository.CalendarSelectionMode.Single, .ShowTodayButton = True}
            AddHandler Me.CalendarControl2.DisableCalendarDate, AddressOf CalendarControl2_DisableCalendarDate
            Me.Controls.Add(Me.CalendarControl2)
            Me.CalendarControl2.BorderStyle = BorderStyles.NoBorder
            Me.CalendarControl2.Location = New Point(Me.Calendar.Location.X + Me.Calendar.Width + 2, Me.Calendar.Location.Y + 1)
        End If
        Me.Size = Me.RangeFormSize()
        _RangeFormInited = True
    End Sub

    Protected Overrides Sub OnCalendar_EditDateModified(sender As Object, e As EventArgs)
        MyBase.OnCalendar_EditDateModified(sender, e)
        If Me.CalendarControl2 IsNot Nothing Then
            Me.CalendarControl2.Update()
            Me.CalendarControl2.Refresh()
        End If
    End Sub

    Private Sub CalendarControl2_DisableCalendarDate(sender As Object, e As DisableCalendarDateEventArgs)
        e.IsDisabled = e.Date < Me.Calendar.DateTime
    End Sub

    Public Sub FixButtonsLocations()
        Me.OkButton.Location = New Point(Me.Width - Me.OkButton.Width - 2, Me.Calendar.Height + 3)
        Me.CancelButton.Location = New Point(Me.OkButton.Location.X - Me.CancelButton.Width - 3, Me.OkButton.Location.Y)
    End Sub

    Public Sub New(dateEdit As DateCriteriaEdit)
        MyBase.New(dateEdit)
        Me.Init()
        Me.IsRangeCheckEdit.Visible = False
        Me.EditMode = dateEdit.Properties.EditMode
        If dateEdit.Criteria IsNot Nothing Then
            Me.Criteria = dateEdit.Criteria
        End If
    End Sub

    Public Sub New(editMode As EditModeEnum)
        MyBase.New(New DateEdit)
        Me.Init()
        Me.EditMode = editMode
    End Sub


    Public Sub New(criteria As CriteriaOperator)
        MyBase.New(New DateEdit)
        Me.Init()
        Me.IsRangeCheckEdit.Visible = False
        Me.SetCriteria(criteria)
    End Sub

    Public Overrides Function CalcFormSize(contentSize As Size) As Size
        If _OriginalSize = Size.Empty Then
            Return MyBase.CalcFormSize(contentSize)
        ElseIf Me.EditMode = EditModeEnum.Between
            Return Me.RangeFormSize
        Else
            Return _OriginalSize
        End If
    End Function


    Private Sub Init()
        Me.Calendar.ShowClearButton = False
        With Me.CalcFormSize()
            _OriginalSize = New Size(.Width, .Height + 26)
        End With

        'Me.Size = _OriginalSize
        _IsRangeCheckEdit = New CheckEdit With {.Text = "Is Range", .Location = New Point(Me.Calendar.Location.X + 3, Me.Calendar.Height + 4)}
        Me.Controls.Add(Me.IsRangeCheckEdit)

        _OkButton = New SimpleButton With {.Text = "Ok", .Size = New Size(100, 22)}
        AddHandler Me.OkButton.Click, AddressOf OkButton_Click
        Me.Controls.Add(Me.OkButton)
        _CancelButton = New SimpleButton With {.Text = "Cancel", .Size = New Size(100, 22), .Visible = False}
        AddHandler Me.CancelButton.Click, AddressOf CancelButton_Click
        Me.Controls.Add(Me.CancelButton)
        AddHandler Me.IsRangeCheckEdit.CheckedChanged, AddressOf IsRangeCheckEdit_CheckedChanged

    End Sub

    Protected Overrides ReadOnly Property AllowCloseByEscape As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property CloseUpKey As KeyShortcut
        Get
            Return New KeyShortcut(Keys.Escape)
        End Get
    End Property

    Private Sub CancelButton_Click(sender As Object, e As EventArgs)
        RaiseEvent OnResult(True)
        Me.Close()
    End Sub

    Public Property StartDate As Date
    Public Property DateTime As Date
    Public Property EndDate As Date

    Public ReadOnly Property DateRange As DateRange
        Get
            If Not IsRange Then Return Nothing
            Return New DateRange(Me.StartDate, Me.EndDate)
        End Get
    End Property

    Public ReadOnly Property OkButtonClicked As Boolean
    Private Sub OkButton_Click(sender As Object, e As EventArgs)
        If _OkButtonClicked Then Return
        _OkButtonClicked = True
        If Me.IsRange Then
            Me.StartDate = Me.Calendar.DateTime
            Me.EndDate = Me.CalendarControl2.DateTime
        Else
            Me.DateTime = Me.Calendar.DateTime
            Me.StartDate = Me.Calendar.DateTime
        End If
        RaiseEvent OnResult(False)
        DirectCast(Me.OwnerEdit, DateCriteriaEdit).Criteria = Me.Criteria
        MyBase.ClosePopup(PopupCloseMode.Normal)
        _OkButtonClicked = False
    End Sub


    Protected Overrides Sub ClosePopup(closeMode As PopupCloseMode)
        If closeMode = PopupCloseMode.Normal Then
            DirectCast(Me.OwnerEdit, DateCriteriaEdit).Criteria = Me.Criteria
        End If
        MyBase.ClosePopup(closeMode)

    End Sub

    Private _IsRangeChanging As Boolean
    Private Sub IsRangeCheckEdit_CheckedChanged(sender As Object, e As EventArgs)
        'If _IsRangeChanging Then Return
        RaiseEvent OnIsRangeCheckEditCheckedChanged(Me.IsRangeCheckEdit.CheckState)
        '_IsRangeChanging = True
        Me.IsRange = Me.IsRangeCheckEdit.Checked
        '_IsRangeChanging = False
    End Sub

    Public Shared Function CreateAndShow(editMode As EditModeEnum, pt As Point) As FilterPopupDateEditForm
        Dim frm As New FilterPopupDateEditForm(editMode)
        frm.Show(pt)
        Return frm
    End Function

    Public Overloads Sub Show(pt As Point)
        Me.Location = pt
        Me.Show()
    End Sub

    Protected Overrides Sub OnShown(ByVal e As EventArgs)
        MyBase.OnShown(e)
        If Me.CalendarControl2 IsNot Nothing Then
            Me.CalendarControl2.Update()
        End If
        'Me.Activate()
    End Sub

    'Protected Overrides Sub OnDeactivate(e As EventArgs)
    '    MyBase.OnDeactivate(e)
    '    RaiseEvent OnResult(True)
    '    Me.Close()
    'End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Me.CalendarControl2 IsNot Nothing Then
                RemoveHandler Me.CalendarControl2.DisableCalendarDate, AddressOf CalendarControl2_DisableCalendarDate
                Me.CalendarControl2.Dispose()
                _CalendarControl2 = Nothing
            End If
            If Me.IsRangeCheckEdit IsNot Nothing Then
                RemoveHandler Me.IsRangeCheckEdit.CheckedChanged, AddressOf IsRangeCheckEdit_CheckedChanged
                Me.IsRangeCheckEdit.Dispose()
                _IsRangeCheckEdit = Nothing
            End If
            If Me.OkButton IsNot Nothing Then
                RemoveHandler Me.OkButton.Click, AddressOf OkButton_Click
                Me.OkButton.Dispose()
                _OkButton = Nothing
            End If
            If Me.CancelButton IsNot Nothing Then
                RemoveHandler Me.CancelButton.Click, AddressOf CancelButton_Click
                Me.CancelButton.Dispose()
                _CancelButton = Nothing
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

End Class
