Imports System.Globalization
Imports System.Reflection
Imports AcurSoft.XtraGrid.Views.Grid
Imports DevExpress.Data.Filtering
Imports DevExpress.Utils.Menu
'Imports DevExpress.XtraBars.Forms
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Helpers
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraTab
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.Nodes

Public Class DateFilterPopupEx
    Inherits DateFilterPopup
    Private Const DateCalendarBorders As Integer = 10
    Public _OriginalLocation As Point

    Private _DataAllDates As List(Of Date)

    Public Sub New(ByVal view As ColumnView, ByVal column As GridColumn, ByVal ownerControl As Control, ByVal creator As Object)
        MyBase.New(view, column, ownerControl, creator)
        Dim foo As Object() = DirectCast(view, GridViewEx).GetFilteredValues(column, False)
        If foo IsNot Nothing Then
            _DataAllDates = foo.OfType(Of Date).ToList()
        End If
    End Sub
    Private BetweenWidth As Integer

    Private _DateCalendar1 As DateControlEx
    Private _DateCalendar2 As DateControlEx
    Private _DateCalendar3 As DateControlEx
    Private _GreaterCheckEdit As CheckEdit
    Private _LessCheckEdit As CheckEdit
    Private _BetweenCheckEdit As CheckEdit
    Private _ShowAllCheckEdit As CheckEdit
    Private _SpecificDateCheckEdit As CheckEdit
    Private _AllCheckEdits As List(Of CheckEdit)
    Private _SpecialCheckEdits As List(Of CheckEdit)
    Private _OneDateCheckEdits As List(Of CheckEdit)
    Private _CompareCheckEdits As List(Of CheckEdit)

    Private _DateCalendar As DateControlEx
    Private ReadOnly Property DateCalendar() As DateControlEx
        Get
            If _DateCalendar Is Nothing Then
                _DateCalendar = DateFilterControl.Controls.OfType(Of DateControlEx).FirstOrDefault
            End If
            Return _DateCalendar
        End Get
    End Property


    Private _DateFilterControl As PopupOutlookDateFilterControl
    Private ReadOnly Property DateFilterControl() As PopupOutlookDateFilterControl
        Get
            If _DateFilterControl Is Nothing Then
                _DateFilterControl = item.PopupControl.Controls.OfType(Of PopupOutlookDateFilterControl).FirstOrDefault

                'SetDateFilterControl(item)
            End If
            Return _DateFilterControl
        End Get
    End Property




#Region "Creators"
    Private Shadows item As RepositoryItemPopupContainerEdit
    Private _Tab1 As XtraTabPage
    Private _Tab2 As XtraTabPage
    Private _Tab3 As XtraTabPage
    Private _PanelBetween As PanelControl
    Private _XtraTabControl As XtraTabControl
    Private _TreeList As TreeList

    Public Overridable Function CreateDatesTree() As TreeList
        'customCheck = GetCheckEdit()
        'customCheck.Visible = False
        'If View.treeListSource IsNot Nothing AndAlso DateFilterControl.Controls.Count > 0 Then
        'CreateTreeList()
        'CalcControlsLocation()
        _TreeList = New TreeList()

        'TreeList.Location = treelistLocation

        _TreeList.BeginUpdate()
        _TreeList.Columns.Add()
        _TreeList.Columns(0).Caption = "Date"
        _TreeList.Columns(0).VisibleIndex = 0
        _TreeList.OptionsView.ShowCheckBoxes = True

        AddHandler _TreeList.AfterCheckNode, AddressOf treelist_AfterCheckNode
        _TreeList.EndUpdate()
        CreateDataSourceTreeList()

        Return _TreeList
        'DateFilterControl.Controls.Add(_TreeList)
        'For Each ctrl As Control In DateFilterControl.Controls
        '    Dim ce As CheckEdit = (TryCast(ctrl, CheckEdit))
        '    If ce IsNot Nothing Then
        '        If ce.Text <> View.customName Then
        '            AddHandler ce.CheckedChanged, AddressOf OriginalDateFilterPopup_CheckedChanged
        '        End If
        '    Else
        '        Dim dateControlEx As DateControlEx = (TryCast(ctrl, DateControlEx))
        '        If dateControlEx IsNot Nothing Then
        '            AddHandler dateControlEx.Click, AddressOf dateControlEx_Click
        '        End If
        '    End If
        'Next ctrl
        'item.PopupFormMinSize = New Size(440, 280 + TreeList.Height)
        'End If
        'Return Nothing
    End Function


    Private Sub CreateActiveFilterCriteria(ByVal e As NodeEventArgs)
        Dim listCriteriaOperator As New List(Of CriteriaOperator)()
        _TreeList.NodesIterator.DoLocalOperation(Sub(node) node.CheckState = e.Node.CheckState, e.Node.Nodes)
        _TreeList.NodesIterator.DoOperation(
            Sub(node)
                If node.Level = 0 Then
                    SetCheckNode(node)
                    If node.Checked Then
                        listCriteriaOperator.Add(GetFilterCriteriaByControlState(node))
                    Else
                        _TreeList.NodesIterator.DoLocalOperation(Sub(childNode) AddCriteria(childNode, listCriteriaOperator), node.Nodes)
                    End If
                End If
            End Sub)
        Me.View.ActiveFilterCriteria = GroupOperator.Or(listCriteriaOperator)
    End Sub
    Private Sub AddCriteria(ByVal childNode As TreeListNode, ByVal listCriteriaOperator As List(Of CriteriaOperator))
        If childNode.Level = 1 Then
            If childNode.Checked Then
                listCriteriaOperator.Add(GetFilterCriteriaByControlState(childNode))
            Else
                _TreeList.NodesIterator.DoLocalOperation(Sub(childChildNode)
                                                             If childChildNode.Checked Then
                                                                 listCriteriaOperator.Add(GetFilterCriteriaByControlState(childChildNode))
                                                             End If
                                                         End Sub, childNode.Nodes)
            End If
        End If
    End Sub


    Private Function GetFilterCriteriaByControlState(ByVal node As TreeListNode) As CriteriaOperator
        Return GetBetweenOperatorByName(node)
    End Function


    'Private Sub OriginalDateFilterPopup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim checkEdit As CheckEdit = DirectCast(sender, CheckEdit)
    '    If checkEdit.Checked AndAlso checkEdit.Text <> "Filter by a specific date:" Then
    '        CreateDataSourceTreeList()
    '    End If
    'End Sub
    Protected Overridable Function GetBetweenOperatorByName(ByVal node As TreeListNode) As BetweenOperator
        If node.Level = 0 Then
            Dim firstDay As New Date(DirectCast(node.GetValue(_TreeList.Columns(0)), Integer), 01, 01)
            Dim endDay As New Date(DirectCast(node.GetValue(_TreeList.Columns(0)), Integer), 12, 31)
            Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
        ElseIf node.Level = 1 Then
            Dim firstDay As New Date(DirectCast(node.ParentNode.GetValue(_TreeList.Columns(0)), Integer), GetMonth(node), 01)
            Dim endDay As New Date(DirectCast(node.ParentNode.GetValue(_TreeList.Columns(0)), Integer), GetMonth(node), Date.DaysInMonth(DirectCast(node.ParentNode.GetValue(_TreeList.Columns(0)), Integer), GetMonth(node)))
            Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
        Else
            Dim firstDay As New Date(DirectCast(node.ParentNode.ParentNode.GetValue(_TreeList.Columns(0)), Integer), GetMonth(node.ParentNode), DirectCast(node.GetValue(_TreeList.Columns(0)), Integer))
            Dim endDay As New Date(DirectCast(node.ParentNode.ParentNode.GetValue(_TreeList.Columns(0)), Integer), GetMonth(node.ParentNode), DirectCast(node.GetValue(_TreeList.Columns(0)), Integer))
            Return New BetweenOperator(Me.Column.FieldName, firstDay, endDay)
        End If
    End Function
    Private Function GetMonth(ByVal node As TreeListNode) As Integer
        Dim dt As Date = Date.ParseExact(DirectCast(node.GetValue(_TreeList.Columns(0)), String), "MMMM", CultureInfo.CurrentCulture)
        Return dt.Month
    End Function


    Private Sub treelist_AfterCheckNode(ByVal sender As Object, ByVal e As NodeEventArgs)
        'For Each ctrl As Control In DateFilterControl.Controls
        '    If ctrl.Text = View.customName Then
        '        Dim checkEdit As CheckEdit = CType(ctrl, CheckEdit)
        '        checkEdit.CheckState = CheckState.Checked
        '    End If
        'Next ctrl
        CreateActiveFilterCriteria(e)
    End Sub

    Private Sub CreateDataSourceTreeList()
        If _DataAllDates Is Nothing Then Return
        _TreeList.BeginUnboundLoad()
        _TreeList.ClearNodes()
        Dim parentForRootNodes As TreeListNode = Nothing
        Dim rootNode As TreeListNode = Nothing, childRootNode As TreeListNode = Nothing, childChildRootNode As TreeListNode = Nothing
        Dim filterRowArray = Me.View.GetFilteredValues(Me.Column, False)
        Dim distinctYearsArray = _DataAllDates.Select(Function(d) d.Year).Distinct().ToArray()

        For Each currentYear As Integer In distinctYearsArray
            rootNode = _TreeList.AppendNode(New Object() {currentYear}, parentForRootNodes)
            Dim distinctMonthArray = _DataAllDates.Where(Function(dt) dt.Year = currentYear).Select(Function(dt) dt.ToString("MMMM")).Distinct().ToArray()
            For Each currentMonth As String In distinctMonthArray
                childRootNode = _TreeList.AppendNode(New Object() {currentMonth}, rootNode)
                Dim distinctDayArray = _DataAllDates.Where(Function(dt) dt.Year = currentYear AndAlso dt.ToString("MMMM") = currentMonth).Select(Function(dt) dt.Day).Distinct().ToArray()
                For Each currentDay As Integer In distinctDayArray
                    childChildRootNode = _TreeList.AppendNode(New Object() {currentDay}, childRootNode)
                    If filterRowArray IsNot Nothing Then
                        Dim currentFilter = filterRowArray.OfType(Of Date)().Where(Function(dt) (dt.Year = currentYear) AndAlso (dt.ToString("MMMM") = currentMonth) AndAlso (dt.Day = currentDay)).ToArray()
                        If currentFilter.ToList().Count > 0 Then
                            childChildRootNode.Checked = True
                        End If
                    End If
                Next currentDay
            Next currentMonth
            SetCheckNode(rootNode)
        Next currentYear
        _TreeList.EndUnboundLoad()
    End Sub

    Private Sub SetCheckNode(ByVal node As TreeListNode)
        Dim childCheck As Boolean
        Dim check As Boolean = True
        _TreeList.NodesIterator.DoLocalOperation(
            Sub(childNode)
                childCheck = True
                If childNode.Level = 1 Then
                    _TreeList.NodesIterator.DoLocalOperation(
                    Sub(childChildNode)
                        If Not childChildNode.Checked Then
                            childCheck = False
                            check = False
                        End If
                    End Sub, childNode.Nodes)
                    childNode.Checked = childCheck
                End If
            End Sub, node.Nodes)
        node.Checked = check
    End Sub

    Private _MenuButton As ButtonEdit

    'Protected Overrides Sub OnActivator_CloseUp(sender As Object, e As CloseUpEventArgs)
    '    If e.Value IsNot Nothing Then
    '        Dim x = 0
    '    End If
    '    MyBase.OnActivator_CloseUp(sender, e)

    'End Sub

    Public Overridable Function CreateOutLookDatesMenu() As DXPopupStandardMenu
        Dim _ConfigMenu As New DXPopupStandardMenu(New DXPopupMenu)

        For Each fd In _FilterOutLookDateElements
            Dim mi As New DXMenuCheckItem(fd.Caption)
            If Me.Column.FilterInfo IsNot Nothing Then
                mi.Checked = fd.Criteria.Equals(Me.Column.FilterInfo.FilterCriteria)
            End If
            AddHandler mi.Click,
                Sub(s, a)
                    Me.OnActivator_CloseUp(Me.PopupActivator, New CloseUpEventArgs(Nothing, True, PopupCloseMode.ButtonClick))
                    Me.Column.FilterInfo = New ColumnFilterInfo(fd.Criteria)
                End Sub
            _ConfigMenu.Menu.Items.Add(mi)
        Next
        Return _ConfigMenu
    End Function

    Private _FilterOutLookDateElements As List(Of FilterDateElement)
    Private _ChooseDateEdit As CheckedComboBoxEdit
    Protected Overrides Function CreateRepositoryItem() As RepositoryItemPopupBase
        item = TryCast(MyBase.CreateRepositoryItem(), RepositoryItemPopupContainerEdit)
        'item.PopupControl
        If DateFilterControl.Controls.Count > 0 Then
            _XtraTabControl = New XtraTabControl
            _Tab1 = _XtraTabControl.TabPages.Add()
            _Tab1.Text = "test 1"
            _Tab2 = _XtraTabControl.TabPages.Add()
            _Tab2.Text = "test 2"
            _Tab3 = _XtraTabControl.TabPages.Add()
            _Tab3.Text = "test 3"
            Me.DateFilterControl.Controls.Add(_XtraTabControl)

            _XtraTabControl.Dock = DockStyle.Fill
            _XtraTabControl.BringToFront()

            _DateCalendar1 = CreateCalendar(_DateCalendar1, DateCalendar.SelectionStart, DateCalendar.Top, DateCalendar.Left)
            '_DateCalendar1.Visible = False
            _DateCalendar2 = CreateCalendar(_DateCalendar2, DateCalendar.SelectionStart, _DateCalendar1.Top, DateCalendar.Left + _DateCalendar1.Width)
            _DateCalendar3 = CreateCalendar(_DateCalendar3, DateCalendar.SelectionStart, _DateCalendar1.Top, DateCalendar.Left + _DateCalendar1.Width)
            '_DateCalendar2.Visible = False
            _PanelBetween = New PanelControl
            _PanelBetween.Controls.Add(_DateCalendar1)
            _DateCalendar1.Dock = DockStyle.Left
            _DateCalendar2.Dock = DockStyle.Left
            _DateCalendar2.BringToFront()

            _PanelBetween.Controls.Add(_DateCalendar2)
            _Tab1.Controls.Add(_PanelBetween)

            _AllCheckEdits = DateFilterControl.Controls.OfType(Of CheckEdit).ToList '
            '.FirstOrDefault(Function(q) q.Text = Name)

            _ShowAllCheckEdit = _AllCheckEdits.FirstOrDefault
            _MenuButton = New ButtonEdit
            _MenuButton.Text = "Chronological Filter"
            '_MenuButton.ReadOnly = True
            _MenuButton.Properties.TextEditStyle = TextEditStyles.DisableTextEditor
            _MenuButton.Properties.AllowFocused = False

            _Tab1.Controls.Add(_MenuButton)

            AddHandler _MenuButton.ButtonClick,
                Sub(s, a)
                    Dim editor As ButtonEdit = DirectCast(s, ButtonEdit)
                    Dim bvi As EditorButtonObjectInfoArgs = DirectCast(editor.GetViewInfo(), ButtonEditViewInfo).ButtonInfoByButton(a.Button)

                    Me.CreateOutLookDatesMenu.Show(editor, New Point(bvi.Bounds.Right, bvi.Bounds.Top))

                End Sub


            _MenuButton.Dock = DockStyle.Top
            _MenuButton.BringToFront()


            _SpecificDateCheckEdit = _AllCheckEdits.ElementAt(1)

            _Tab1.Controls.Add(_ShowAllCheckEdit)
            _ShowAllCheckEdit.Dock = DockStyle.Top
            _SpecificDateCheckEdit.BringToFront()
            _Tab1.Controls.Add(_SpecificDateCheckEdit)
            _SpecificDateCheckEdit.Dock = DockStyle.Top
            _SpecificDateCheckEdit.BringToFront()

            _CompareCheckEdits = New List(Of CheckEdit)
            _GreaterCheckEdit = _AllCheckEdits.FirstOrDefault(Function(q) q.Text = Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater))
            _CompareCheckEdits.Add(_GreaterCheckEdit)
            _LessCheckEdit = _AllCheckEdits.FirstOrDefault(Function(q) q.Text = Localizer.Active.GetLocalizedString(StringId.FilterClauseLess))
            _CompareCheckEdits.Add(_LessCheckEdit)
            _BetweenCheckEdit = _AllCheckEdits.FirstOrDefault(Function(q) q.Text = Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween))
            _CompareCheckEdits.Add(_BetweenCheckEdit)

            For Each ce In _CompareCheckEdits
                _Tab1.Controls.Add(ce)
                ce.Dock = DockStyle.Top
                ce.BringToFront()
            Next
            _Tab1.Controls.Add(DateCalendar)
            DateCalendar.Dock = DockStyle.Top
            DateCalendar.BringToFront()
            DateCalendar.Visible = False

            _Tab1.Controls.Add(_DateCalendar3)
            _DateCalendar3.Dock = DockStyle.Top
            _DateCalendar3.BringToFront()
            _DateCalendar3.Visible = False

            _PanelBetween.Dock = DockStyle.Top
            _PanelBetween.Height = _DateCalendar.Height '+ 50
            _PanelBetween.BringToFront()
            _PanelBetween.Visible = False

            If Me.CreateDatesTree() IsNot Nothing Then
                _Tab2.Controls.Add(_TreeList)
                _TreeList.Dock = DockStyle.Fill
                _TreeList.BringToFront()
            End If

            Dim n As Integer = 0
            '_GreaterCheckEdit.Location = New Point(_ShowAllCheckEdit.Location.X, _ShowAllCheckEdit.Location.Y + _ShowAllCheckEdit.Size.Height * 2)
            '_LessCheckEdit.Location = New Point(_ShowAllCheckEdit.Location.X, _ShowAllCheckEdit.Location.Y + _ShowAllCheckEdit.Size.Height * 3)
            '_BetweenCheckEdit.Location = New Point(_ShowAllCheckEdit.Location.X, _ShowAllCheckEdit.Location.Y + _ShowAllCheckEdit.Size.Height * 4)
            'DateCalendar.Location = New Point(_ShowAllCheckEdit.Location.X, _ShowAllCheckEdit.Location.Y + _ShowAllCheckEdit.Size.Height * 5)
            '_DateCalendar1.Location = DateCalendar.Location
            '_DateCalendar2.Location = New Point(_ShowAllCheckEdit.Location.X + DateCalendar.Size.Width, DateCalendar.Location.Y)

            _OneDateCheckEdits = New List(Of CheckEdit)
            With _OneDateCheckEdits
                .Add(_SpecificDateCheckEdit)
                .Add(_GreaterCheckEdit)
                .Add(_LessCheckEdit)
            End With

            For Each ce In _OneDateCheckEdits
                AddHandler ce.CheckedChanged,
                    Sub(s, a)
                        If _ShowingCalender Then Return
                        _ShowingCalender = True
                        Dim edit As CheckEdit = DirectCast(s, CheckEdit)

                        If edit.Checked Then
                            If edit Is _SpecificDateCheckEdit Then
                                Me.DateCalendar.Visible = True
                                _DateCalendar3.Visible = False
                                _GreaterCheckEdit.Checked = False
                                _LessCheckEdit.Checked = False
                            ElseIf edit Is _GreaterCheckEdit Then
                                Me.DateCalendar.Visible = False
                                _DateCalendar3.Visible = True
                                _SpecificDateCheckEdit.Checked = False
                                _LessCheckEdit.Checked = False
                            ElseIf edit Is _LessCheckEdit Then
                                Me.DateCalendar.Visible = False
                                _DateCalendar3.Visible = True
                                _SpecificDateCheckEdit.Checked = False
                                _GreaterCheckEdit.Checked = False
                            End If
                            _BetweenCheckEdit.Checked = False

                        Else
                            Me.DateCalendar.Visible = False
                            If edit Is _SpecificDateCheckEdit Then
                                Me.DateCalendar.Visible = False
                            ElseIf edit Is _GreaterCheckEdit Then
                                _DateCalendar3.Visible = False
                            ElseIf edit Is _LessCheckEdit Then
                                _DateCalendar3.Visible = False
                            End If
                        End If
                        'Me.DateCalendar.Visible = _OneDateCheckEdits.FirstOrDefault(Function(q) q.Checked) IsNot Nothing
                        _ShowingCalender = False
                    End Sub
            Next

            AddHandler _ShowAllCheckEdit.CheckedChanged,
                Sub(s, a)
                    If _ShowAllCheckEdit.Checked Then
                        Me.DateCalendar.Visible = False
                        _DateCalendar3.Visible = False
                        _PanelBetween.Visible = False
                    End If
                End Sub

            AddHandler _BetweenCheckEdit.CheckedChanged,
                Sub(s, a)
                    If _BetweenCheckEdit.Checked Then
                        Dim edit As CheckEdit = _OneDateCheckEdits.FirstOrDefault(Function(q) q.Checked)
                        If edit IsNot Nothing Then
                            edit.Checked = False
                        End If

                        _PanelBetween.Visible = True
                        Dim location As Point = FindOwnerForm(Me.DateFilterControl).Location
                        _OriginalLocation = location
                        BetweenWidth = FindOwnerForm(Me.DateFilterControl).Width

                        FindOwnerForm(Me.DateFilterControl).Width = _DateCalendar1.Width + _DateCalendar2.Width + DateCalendarBorders * 4
                        If Not Screen.PrimaryScreen.Bounds.Contains(FindOwnerForm(Me.DateFilterControl).Bounds) Then
                            location.X -= FindOwnerForm(_DateFilterControl).Width \ 2 - DateCalendarBorders * 2
                        End If
                        FindOwnerForm(_DateFilterControl).Location = location
                        Me.View.ActiveFilterCriteria = GetFilterCriteriaByControlState()

                    Else
                        _PanelBetween.Visible = False
                        ReturnOriginalView()
                    End If
                End Sub


            _SpecialCheckEdits = _OneDateCheckEdits.ToList
            With _SpecialCheckEdits
                .Add(_ShowAllCheckEdit)
                '.Add(_SpecificDateCheckEdit)
                '.Add(_GreaterCheckEdit)
                '.Add(_LessCheckEdit)
                .Add(_BetweenCheckEdit)
            End With

            '_FilterOutLookDateElements = New List(Of FilterDateElement)
            _FilterOutLookDateElements = _AllCheckEdits.Where(Function(q) Not _SpecialCheckEdits.Contains(q) AndAlso q.Tag IsNot Nothing AndAlso TypeOf q.Tag Is FilterDateElement).Select(Function(q) DirectCast(q.Tag, FilterDateElement)).ToList()

            'For Each checkEdit In _AllCheckEdits
            '    If Not _SpecialCheckEdits.Contains(checkEdit) Then
            '        If checkEdit.Tag IsNot Nothing AndAlso TypeOf checkEdit.Tag Is FilterDateElement Then
            '            _FilterOutLookDateElements.Add(DirectCast(checkEdit.Tag, FilterDateElement))
            '        End If
            '        checkEdit.Visible = False
            '        'AddHandler checkEdit.CheckedChanged, AddressOf OriginalDateFilterPopup_CheckedChanged
            '    End If
            'Next
            'AddHandler _BetweenCheckEdit.CheckedChanged, AddressOf CheckedChanged
            'AddHandler _GreaterCheckEdit.CheckedChanged, AddressOf CheckedChanged
            'AddHandler _LessCheckEdit.CheckedChanged, AddressOf CheckedChanged
        End If
        Return item
    End Function

    Private _ShowingCalender As Boolean


    Private Function FindOwnerForm(ByVal Owner As Control) As Control
        If TypeOf Owner.Parent Is PopupContainerForm Then
            Return Owner.Parent
        Else
            Return FindOwnerForm(Owner.Parent)
        End If
    End Function

    Private Sub ReturnOriginalView()
        '_DateCalendar1.Visible = False
        '_DateCalendar2.Visible = False
        '_PanelBetween.Visible = False
        '_DateCalendar.Visible = True
        FindOwnerForm(DateFilterControl).Location = _OriginalLocation
        If BetweenWidth < DateFilterControl.Parent.Parent.Width Then
            DateFilterControl.Parent.Parent.Width = BetweenWidth
        End If
    End Sub
    Private Function CreateCalendar(ByVal calendar As DateControlEx, ByVal dateTime As Date, ByVal top As Integer, ByVal left As Integer) As DateControlEx
        calendar = New DateControlEx() With {
            .DateTime = dateTime,
            .Top = top,
            .Left = left}
        AddHandler calendar.EditValueChanged, AddressOf DateCalendar_SelectionChanged
        DateFilterControl.Controls.Add(calendar)
        Return calendar
    End Function
#End Region

#Region "Handlers &HandlersHelpers"




    Private Sub CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim checkEdit As CheckEdit = DirectCast(sender, CheckEdit)
        If checkEdit.Checked Then
            'If checkEdit Is _BetweenCheckEdit Then
            '    '_LessCheckEdit.Checked = False
            '    _GreaterCheckEdit.Checked = False
            'ElseIf checkEdit Is _LessCheckEdit
            '    '_GreaterCheckEdit.Checked = False
            '    _BetweenCheckEdit.Checked = False
            'ElseIf checkEdit Is _GreaterCheckEdit
            '    '_LessCheckEdit.Checked = False
            '    _BetweenCheckEdit.Checked = False
            'End If
            For Each ctrl As CheckEdit In _AllCheckEdits
                If Not _CompareCheckEdits.Contains(checkEdit) Then
                    checkEdit.Checked = False
                End If
            Next ctrl

            'UpdateOurControlCheckedState(checkEdit.Text)
            'CalcControlsLocation(checkEdit.Text)

            'DateCalendar.Visible = False
            '_DateCalendar1.Visible = True
            'Dim Location As Point = FindOwnerForm(Me.DateFilterControl).Location
            '_OriginalLocation = Location
            'BetweenWidth = FindOwnerForm(Me.DateFilterControl).Width
            'If checkEdit Is _BetweenCheckEdit Then
            '    _PanelBetween.Visible = True
            '    FindOwnerForm(Me.DateFilterControl).Width = _DateCalendar1.Width + _DateCalendar2.Width + DateCalendarBorders * 4
            '    _DateCalendar2.Visible = True
            '    If Not Screen.PrimaryScreen.Bounds.Contains(FindOwnerForm(Me.DateFilterControl).Bounds) Then
            '        Location.X -= FindOwnerForm(_DateFilterControl).Width \ 2 - DateCalendarBorders * 2
            '    End If
            '    FindOwnerForm(_DateFilterControl).Location = Location
            'End If




            'Me.View.ActiveFilterCriteria = GetFilterCriteriaByControlState()
        Else
            If _DateCalendar1.Visible OrElse _DateCalendar2.Visible Then
                ReturnOriginalView()
                'ReturnOriginalControlsLocation()
            End If
        End If

    End Sub

    Private Sub OriginalDateFilterPopup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim checkEdit As CheckEdit = DirectCast(sender, CheckEdit)
        If checkEdit.Checked Then
            '_GreaterCheckEdit.Checked = False
            '_LessCheckEdit.Checked = False
            '_BetweenCheckEdit.Checked = False
            If checkEdit IsNot _SpecificDateCheckEdit Then
                CreateDataSourceTreeList()
            End If
        End If

    End Sub
    'Private Sub ReturnOriginalControlsLocation()
    '    For i As Integer = 0 To DateFilterControl.Controls.Count - 2
    '        Dim ctrl As Control = DateFilterControl.Controls(i + 1)
    '        Dim NewLocation As Point = DateFilterControl.Controls(i).Location
    '        NewLocation.Y += DateFilterControl.Controls(i).Height
    '        ctrl.Location = NewLocation
    '    Next i
    'End Sub

    Private Sub DateCalendar_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.View.ActiveFilterCriteria = GetFilterCriteriaByControlState()
        If (Not _GreaterCheckEdit.Checked) AndAlso (Not _LessCheckEdit.Checked) And (Not _BetweenCheckEdit.Checked) Then
            _DateCalendar.DateTime = _DateCalendar1.DateTime
        End If
    End Sub
#End Region


    Private Function GetFilterCriteriaByControlState() As CriteriaOperator
        If _DateCalendar1 IsNot Nothing Then
            If _GreaterCheckEdit.Checked Then
                Return New BinaryOperator(Me.Column.FieldName, _DateCalendar3.DateTime, BinaryOperatorType.Greater)

                'Return GetBinaryOperatorByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater))
            End If

            If _LessCheckEdit.Checked Then
                Return New BinaryOperator(Me.Column.FieldName, _DateCalendar3.DateTime, BinaryOperatorType.Less)
                'Return GetBinaryOperatorByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseLess))
            End If

            If _DateCalendar2 IsNot Nothing Then
                If _BetweenCheckEdit.Checked Then
                    Return New BetweenOperator(Me.Column.FieldName, _DateCalendar1.DateTime, _DateCalendar2.DateTime)
                    'Return GetBetweenOperatorByName(Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween))
                End If
                'If Name = Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween) Then
                'End If
            End If
        End If

        Return Nothing
    End Function
    Protected Shadows ReadOnly Property View() As GridViewEx
        Get
            Return TryCast(MyBase.View, GridViewEx)
        End Get
    End Property

#Region "ControlHelpers"


    'Protected Overridable Function GetBinaryOperatorByName(ByVal Name As String) As BinaryOperator
    '    If _DateCalendar1 IsNot Nothing Then
    '        If Name = Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater) Then
    '            Return New BinaryOperator(Me.Column.FieldName, _DateCalendar1.DateTime, BinaryOperatorType.Greater)
    '        End If
    '        If Name = Localizer.Active.GetLocalizedString(StringId.FilterClauseLess) Then
    '            Return New BinaryOperator(Me.Column.FieldName, _DateCalendar1.DateTime, BinaryOperatorType.Less)
    '        End If
    '    End If
    '    Return Nothing
    'End Function
    'Protected Overridable Function GetBetweenOperatorByName(ByVal Name As String) As BetweenOperator
    '    If _DateCalendar2 IsNot Nothing Then
    '        If Name = Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween) Then
    '            Return New BetweenOperator(Me.Column.FieldName, _DateCalendar1.DateTime, _DateCalendar2.DateTime)
    '        End If
    '    End If
    '    Return Nothing

    'End Function
#End Region

    'Protected Overridable Function NotOurControl(ByVal ctrl As CheckEdit) As Boolean
    '    If ctrl.Text <> Localizer.Active.GetLocalizedString(StringId.FilterClauseGreater) AndAlso ctrl.Text <> Localizer.Active.GetLocalizedString(StringId.FilterClauseLess) AndAlso ctrl.Text <> Localizer.Active.GetLocalizedString(StringId.FilterClauseBetween) Then
    '        Return True
    '    End If

    '    Return False
    'End Function

    Public Overrides Sub Dispose()
        If _DateCalendar1 IsNot Nothing Then
            RemoveHandler _DateCalendar1.EditValueChanged, AddressOf DateCalendar_SelectionChanged
        End If
        If _DateCalendar2 IsNot Nothing Then
            RemoveHandler _DateCalendar2.EditValueChanged, AddressOf DateCalendar_SelectionChanged
        End If

        For Each checkEdit In _AllCheckEdits
            If Not _CompareCheckEdits.Contains(checkEdit) Then
                RemoveHandler checkEdit.CheckedChanged, AddressOf OriginalDateFilterPopup_CheckedChanged
            End If
        Next
        Try
            Me.View.ActiveFilterString = GetFilterCriteriaByControlState().ToString()
        Catch
        End Try
        _DateCalendar1.Dispose()
        _DateCalendar2.Dispose()
        MyBase.Dispose()
        If _DateCalendar IsNot Nothing Then
            _DateCalendar.Dispose()
            _DateCalendar = Nothing
        End If
        If _GreaterCheckEdit IsNot Nothing Then
            _GreaterCheckEdit.Dispose()
            _GreaterCheckEdit = Nothing
        End If
        If _LessCheckEdit IsNot Nothing Then
            _LessCheckEdit.Dispose()
            _LessCheckEdit = Nothing
        End If
        If _BetweenCheckEdit IsNot Nothing Then
            _BetweenCheckEdit.Dispose()
            _BetweenCheckEdit = Nothing
        End If
        If _PanelBetween IsNot Nothing Then
            _PanelBetween.Dispose()
            _PanelBetween = Nothing
        End If
        If _XtraTabControl IsNot Nothing Then
            _XtraTabControl.Dispose()
            _XtraTabControl = Nothing
        End If

        If _DateFilterControl IsNot Nothing Then
            _DateFilterControl.Dispose()
            _DateFilterControl = Nothing
        End If
        If item IsNot Nothing Then
            item.Dispose()
            item = Nothing
        End If
    End Sub
End Class

