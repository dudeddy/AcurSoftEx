Imports AcurSoft.XtraGrid.Views.Grid
Imports DevExpress.Data
Imports DevExpress.Utils
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid.Controls
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraLayout
Imports AcurSoft.XtraEditors
Imports AcurSoft.XtraGrid.Views.Grid.Bookmarks
Imports DevExpress.XtraGrid.Extension
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.FilterEditor
Imports DevExpress.XtraEditors.Frames
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns

Namespace AcurSoft.XtraGrid.Controls

    Public Class FindControlEx
        Inherits FindControl
        Public ReadOnly Property ButtonClose As SimpleButton
        Public ReadOnly Property ButtonClear As SimpleButton
        Public ReadOnly Property ButtonFind As SimpleButton
        Public ReadOnly Property EditorFind As ButtonEdit
        Public ReadOnly Property ButtonConfig As ButtonEdit
        Public ReadOnly Property ButtonBookmarks As ButtonEdit
        Public ReadOnly Property ButtonBookmarkShowFiltredOnly As EditorButton
        Public ReadOnly Property ButtonBookmarkShowAllRows As EditorButton
        Public ReadOnly Property ButtonBookmarkShowSelected As EditorButton
        Public ReadOnly Property LciBookmarks As LayoutControlItem
        Public ReadOnly Property MainLayoutControl As LayoutControl
        'Public ReadOnly Property FindHelper As FindHelper
        Public ReadOnly Property ColsCheckedComboBoxEdit As CheckedComboBoxEdit
        Private _ViewEx As GridViewEx

        Public Function GetCheckedColumns() As List(Of IDataColumnInfo)
            Return Me.ColsCheckedComboBoxEdit.Properties.Items.Where(Function(q) q.CheckState = CheckState.Checked).Select(Function(q) DirectCast(q.Tag, IDataColumnInfo)).ToList()
        End Function


        Public Overridable Function CreateConfigMenu() As DXPopupMenu
            Dim menu As New DXPopupMenu
            Dim msiGroups As New DXSubMenuItem("Groups")
            menu.Items.Add(msiGroups)

            Dim miGrid As New DXMenuCheckItem() With {
                .Caption = "Group Panel", .Checked = _ViewEx.OptionsView.ShowGroupPanel}
            AddHandler miGrid.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.ShowGroupPanel = Not _ViewEx.OptionsView.ShowGroupPanel
                End Sub
            msiGroups.Items.Add(miGrid)

            miGrid = New DXMenuCheckItem() With {
                .Caption = "Group Panel columns as a single row",
                .Checked = _ViewEx.OptionsView.ShowGroupPanelColumnsAsSingleRow,
                .Enabled = _ViewEx.OptionsView.ShowGroupPanel
            }

            AddHandler miGrid.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.ShowGroupPanelColumnsAsSingleRow = Not _ViewEx.OptionsView.ShowGroupPanelColumnsAsSingleRow
                End Sub
            msiGroups.Items.Add(miGrid)


            '_ConfigMenu.Menu.Items.Add(New DXMenuItem("text"))

            Dim msiColums As New DXSubMenuItem("Columns")
            menu.Items.Add(msiColums)

            Dim miColumnTool As New DXMenuItem With {.Caption = "Best Fit (all columns)"}
            AddHandler miColumnTool.Click,
                Sub(s, a)
                    _ViewEx.BestFitColumns()
                End Sub
            msiColums.Items.Add(miColumnTool)

            miColumnTool = New DXMenuItem With {.Caption = "Column Chooser"}
            AddHandler miColumnTool.Click,
                Sub(s, a)
                    _ViewEx.ShowCustomization()
                End Sub
            msiColums.Items.Add(miColumnTool)



            Dim miColumn As New DXMenuCheckItem() With {
                .Caption = "Auto Width", .BeginGroup = True, .Checked = _ViewEx.OptionsView.ColumnAutoWidth AndAlso Not _ViewEx.OptionsView.FillEmptySpace}
            AddHandler miColumn.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.FillEmptySpace = False
                    _ViewEx.OptionsView.ColumnAutoWidth = True
                End Sub
            msiColums.Items.Add(miColumn)

            miColumn = New DXMenuCheckItem() With {
                .Caption = "Fill Empty Space", .Checked = Not _ViewEx.OptionsView.ColumnAutoWidth AndAlso _ViewEx.OptionsView.FillEmptySpace}
            AddHandler miColumn.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.FillEmptySpace = True
                End Sub
            msiColums.Items.Add(miColumn)

            miColumn = New DXMenuCheckItem() With {
                .Caption = "Free Size", .Checked = Not _ViewEx.OptionsView.ColumnAutoWidth AndAlso Not _ViewEx.OptionsView.FillEmptySpace}
            AddHandler miColumn.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.FillEmptySpace = False
                    _ViewEx.OptionsView.ColumnAutoWidth = False
                End Sub
            msiColums.Items.Add(miColumn)

            Dim msiFilters As New DXSubMenuItem("Filters")
            menu.Items.Add(msiFilters)
            Dim miFilter As New DXMenuCheckItem() With {
                .Caption = "Accent Insensitive Search", .Checked = _ViewEx.OptionsFilter.AccentInsensitive}
            AddHandler miFilter.Click,
                Sub(s, a)
                    _ViewEx.OptionsFilter.AccentInsensitive = Not _ViewEx.OptionsFilter.AccentInsensitive
                End Sub
            msiFilters.Items.Add(miFilter)

            miFilter = New DXMenuCheckItem() With {.Caption = "AutoFilter row", .Checked = _ViewEx.OptionsView.ShowAutoFilterRow}
            AddHandler miFilter.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.ShowAutoFilterRow = Not _ViewEx.OptionsView.ShowAutoFilterRow
                End Sub
            msiFilters.Items.Add(miFilter)
            Dim miSortDialog As New DXMenuItem With {.Caption = "Sort Config", .BeginGroup = True}
            AddHandler miSortDialog.Click,
                Sub(s, a)
                    Using frm As New Extenders.GridSortForm(_ViewEx)
                        If frm.ShowDialog = DialogResult.OK Then
                            frm.SaveChanges()
                        End If
                    End Using
                End Sub
            menu.Items.Add(miSortDialog)

            Dim miShowFooter As New DXMenuCheckItem() With {
                .Caption = "Show Footer", .Checked = _ViewEx.OptionsView.ShowFooter,
                .BeginGroup = True}
            AddHandler miShowFooter.Click,
                Sub(s, a)
                    _ViewEx.OptionsView.ShowFooter = Not _ViewEx.OptionsView.ShowFooter
                End Sub
            menu.Items.Add(miShowFooter)

            If _ViewEx.OptionsMenu.ShowConditionalFormattingItem Then
                Dim miShowConditionalFormattingMenuItem As New DXMenuItem("Manage Conditional Formatting Rules")
                AddHandler miShowConditionalFormattingMenuItem.Click,
                    Sub(s, a)
                        _ViewEx.Columns(0).ShowFormatRulesManager()

                    End Sub
                menu.Items.Add(miShowConditionalFormattingMenuItem)
                If Me.View.FormatRules.Count > 0 Then
                    Dim miClearAllConditionalFormattingMenuItem As New DXMenuItem("Clear all Conditional Formatting Rules")
                    AddHandler miClearAllConditionalFormattingMenuItem.Click,
                        Sub(s, a)
                            View.BeginUpdate()
                            Try
                                For i As Integer = View.FormatRules.Count - 1 To 0 Step -1
                                    View.FormatRules.RemoveAt(i)
                                Next i
                            Finally
                                View.EndUpdate()
                            End Try
                        End Sub
                    menu.Items.Add(miClearAllConditionalFormattingMenuItem)

                End If
            End If

            Return menu
        End Function


        Public Overridable Function CreateBookMarksMenu() As DXPopupMenu
            If Not _ViewEx.OptionsBehavior.BookmarksHelper.IsInitiated Then Return Nothing
            Dim bkms As GridViewBookmarks = _ViewEx.OptionsBehavior.BookmarksHelper
            'If bkms.BookMarks.Count = 0 Then Return Nothing

            Dim menu As New DXPopupMenu
            'Dim menu As New DXPopupStandardMenu(New DXPopupMenu)

            'Dim v As Object = Me.View.GetFocusedRowCellValue(bkms.KeyFieldName)
            Dim frh As Integer = Me.View.FocusedRowHandle

            'Dim itemAdd As DXMenuItem = bkms.CreateAddBookmarkMenuItem(frh)
            'If itemAdd IsNot Nothing Then
            '    menu.Items.Add(itemAdd)
            'End If
            Dim itemAdd As DXSubMenuItem = bkms.CreateAddBookmarkSubMenuItem(frh)
            If itemAdd IsNot Nothing Then
                menu.Items.Add(itemAdd)
            End If
            Dim removeItem As DXMenuItem = bkms.CreateRemoveBookmarkMenuItem(frh)
            If removeItem IsNot Nothing Then
                removeItem.BeginGroup = itemAdd IsNot Nothing
                menu.Items.Add(removeItem)

                Dim colorsSubItems As DXSubMenuItem = bkms.CreateSetBookmarkColorMenuItem(frh)
                If colorsSubItems IsNot Nothing Then
                    colorsSubItems.BeginGroup = True
                    menu.Items.Add(colorsSubItems)
                End If
            End If
            Dim clearItem As DXMenuItem = bkms.CreateClearBookmarksMenuItem()
            If clearItem IsNot Nothing Then
                clearItem.BeginGroup = True
                menu.Items.Add(clearItem)
                _ViewEx.OptionsBehavior.BookmarksHelper.CreateFilterBookmarksMenuItems(menu.Items, True)

            End If
            Dim bookMarkedCount As Integer = bkms.BookMarks.Count
            'Dim bmk As BookmarkData = bkms.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))
            If bookMarkedCount > 0 Then
                If Not (bookMarkedCount = 1 AndAlso bkms.BookMarks.FirstOrDefault.Value.Equals(Me.View.GetFocusedRowCellValue(bkms.KeyFieldName))) Then
                    Dim gotoSubLabelItem As New DXMenuHeaderItem() With {.Caption = "Go to bookmark", .BeginGroup = True}
                    menu.Items.Add(gotoSubLabelItem)
                    bkms.CreateGotoBookmarksMenuItems(menu.Items, True)
                End If
            End If

            Return menu
        End Function

        Public Sub New(ByVal view As GridViewEx, ByVal properties As Object)
            MyBase.New(view, properties)
            _ViewEx = view
            'view.FindControl = Me
            _MainLayoutControl = layoutControl1
            _MainLayoutControl.Root.Padding = New Utils.Padding(3)
            _ButtonClose = DirectCast(FindControl("btClose"), SimpleButton)
            _ButtonClear = DirectCast(FindControl("btClear"), SimpleButton)
            _ButtonFind = DirectCast(FindControl("btFind"), SimpleButton)
            _EditorFind = TryCast(FindControl("teFind"), ButtonEdit)
            Me.Size = New Size(Me.Width, Me.Height - 14)
            'FindHelper = New FindHelper() With {.TargetView = view, .SearchControl = Me.FindEdit}
            Me.Init()
            lciCloseButton.HideToCustomization()
            lciClearButton.HideToCustomization()
            lciFindButton.HideToCustomization()
            _ColsCheckedComboBoxEdit = New CheckedComboBoxEdit
            With _ColsCheckedComboBoxEdit.Properties.Buttons(0)
                .Kind = ButtonPredefines.Glyph
                .ImageLocation = ImageLocation.MiddleCenter
                .Image = Ressources.table_field_16x16
            End With

            AddHandler _ColsCheckedComboBoxEdit.EditValueChanged, AddressOf ColsCheckedComboBoxEdit_EditValueChanged


            Dim lciCols As New LayoutControlItem() With {.Text = "Find in:", .Control = _ColsCheckedComboBoxEdit, .Name = "lciCols"}
            With lciCols
                .Padding = New Utils.Padding(2)
                .MaxSize = New Size(200, 0)
                .MinSize = New Size(200, 26)
                .Size = .MinSize
            End With
            lciCols.SizeConstraintsType = SizeConstraintsType.Custom
            _MainLayoutControl.Root.Add(lciCols)

            For Each c In _ViewEx.GetFindColumns
                With _ColsCheckedComboBoxEdit.Properties.Items
                    .Add(New CheckedListBoxItem() With {.Description = c.Caption, .Value = c.FieldName, .Tag = c})
                End With
            Next
            _ColsCheckedComboBoxEdit.CheckAll()

            Dim lciEmpty As New EmptySpaceItem() With {.Name = "lciEmpty"}


            _MainLayoutControl.Root.Add(lciEmpty)
            lciEmpty.Move(lciFind, Utils.InsertType.Right)

            _ButtonConfig = New ButtonEdit With {.Name = "ButtonConfig"}
            With _ButtonConfig.Properties
                '.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
                .TextEditStyle = TextEditStyles.HideTextEditor
                .Buttons(0).Tag = "Config"
                .Buttons(0).Kind = ButtonPredefines.Glyph
                .Buttons(0).ImageLocation = ImageLocation.MiddleCenter
                .Buttons(0).Image = Ressources.settings_options_16x16
            End With

            AddHandler _ButtonConfig.ButtonClick, AddressOf ButtonConfig_ButtonClick

            Dim lciConfig As New LayoutControlItem() With {.Text = "Config", .Control = _ButtonConfig, .TextVisible = False, .Name = "lciConfig"}
            lciConfig.SizeConstraintsType = SizeConstraintsType.Custom
            With lciConfig
                .Padding = New Utils.Padding(2)
                .Size = New Size(24, 24)
                .MaxSize = New Size(24, 24)
                .MinSize = New Size(24, 24)
                '.ControlMaxSize = New Size(48, 24)
                '.ControlMinSize = New Size(48, 24)
            End With
            _MainLayoutControl.Root.Add(lciConfig)
            lciConfig.Move(lciEmpty, Utils.InsertType.Left)

            _ButtonBookmarks = New ButtonEdit With {.Name = "ButtonBookmarks"}
            _ButtonBookmarkShowFiltredOnly = New EditorButton With {
                .Tag = "Filter",
                .Kind = ButtonPredefines.Glyph,
                .ImageLocation = ImageLocation.MiddleCenter,
                .Image = Ressources.bookmark_filter_16x16,
                .Enabled = False,
                .ToolTip = "Show Bookmarks Only"
            }
            _ButtonBookmarkShowAllRows = New EditorButton With {
                .Tag = "AllRows",
                .Kind = ButtonPredefines.Glyph,
                .ImageLocation = ImageLocation.MiddleCenter,
                .Image = Ressources.bookmark_filter_deabled_16x16,
                .Enabled = False,
                .ToolTip = "Show All Rows"
            }
            _ButtonBookmarkShowSelected = New EditorButton With {
                .Tag = "Selected",
                .Kind = ButtonPredefines.Glyph,
                .ImageLocation = ImageLocation.MiddleCenter,
                .Image = Ressources.bookmark_filter_select_16x16,
                .Enabled = False,
                .ToolTip = "Show Selected Flags"
            }

            With _ButtonBookmarks.Properties
                '.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
                .TextEditStyle = TextEditStyles.HideTextEditor
                .Buttons(0).Tag = "Bookmarks"
                .Buttons(0).Kind = ButtonPredefines.Glyph
                .Buttons(0).ImageLocation = ImageLocation.MiddleCenter
                .Buttons(0).Image = Ressources.bookmark_16x16
                .Buttons(0).ToolTip = "Bookmarks / Flags"
                .Buttons.Add(_ButtonBookmarkShowFiltredOnly)
                .Buttons.Add(_ButtonBookmarkShowAllRows)
                .Buttons.Add(_ButtonBookmarkShowSelected)
            End With
            AddHandler _ButtonBookmarks.ButtonClick, AddressOf ButtonBookmarks_ButtonClick

            _LciBookmarks = New LayoutControlItem() With {.Text = "Bookmarks", .Control = _ButtonBookmarks, .TextVisible = False, .Name = "lciBookmarks"}
            _LciBookmarks.SizeConstraintsType = SizeConstraintsType.Custom
            With _LciBookmarks
                .Padding = New Utils.Padding(2)
                .Size = New Size(24, 24)
                .MaxSize = New Size(96, 24)
                .MinSize = New Size(96, 24)
                '.ControlMaxSize = New Size(48, 24)
                '.ControlMinSize = New Size(48, 24)
            End With
            _MainLayoutControl.Root.Add(_LciBookmarks)
            _LciBookmarks.Move(lciEmpty, Utils.InsertType.Left)



            lciFind.Move(lciEmpty, Utils.InsertType.Right)
            lciCols.Move(lciFind, Utils.InsertType.Left)
        End Sub

        Private Sub ButtonBookmarks_ButtonClick(sender As Object, e As ButtonPressedEventArgs)
            If e.Button.Tag Is Nothing Then Return
            Dim editor As ButtonEdit = DirectCast(sender, ButtonEdit)
            Select Case e.Button.Tag.ToString
                Case "Bookmarks"
                    Dim bvi As EditorButtonObjectInfoArgs = DirectCast(editor.GetViewInfo(), ButtonEditViewInfo).ButtonInfoByButton(e.Button)
                    Dim menu As DXPopupMenu = Me.CreateBookMarksMenu
                    If menu IsNot Nothing Then
                        editor.ShowMenu(e.Button, menu)
                    End If
                Case "Filter"
                    Dim h As GridViewBookmarks = _ViewEx.OptionsBehavior.BookmarksHelper
                    If Not h.IsInitiated Then Return
                    h.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.AllFlags)
                Case "AllRows"
                    Dim h As GridViewBookmarks = _ViewEx.OptionsBehavior.BookmarksHelper
                    If Not h.IsInitiated Then Return
                    h.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.None)
                Case "Selected"
                    Dim h As GridViewBookmarks = _ViewEx.OptionsBehavior.BookmarksHelper
                    If Not h.IsInitiated Then Return
                    h.SelectFlags()
            End Select
        End Sub

        Private Sub ButtonConfig_ButtonClick(sender As Object, e As ButtonPressedEventArgs)
            If e.Button.Tag Is Nothing Then Return
            Dim editor As ButtonEdit = DirectCast(sender, ButtonEdit)
            Select Case e.Button.Tag.ToString
                Case "Config"
                    editor.ShowMenu(e.Button, Me.CreateConfigMenu)
            End Select
        End Sub


        Private Sub ColsCheckedComboBoxEdit_EditValueChanged(sender As Object, e As EventArgs)
            _ViewEx.OptionsFind.FindFilterColumns = ColsCheckedComboBoxEdit.Text
            If Not String.IsNullOrEmpty(Me.SearchEditor.Text) Then
                Dim findText As String = Me.SearchEditor.Text
                _ViewEx.ApplyFindFilter("")
                Me.PerformSearch(findText)
                _ViewEx.ApplyFindFilter(findText)
            End If
        End Sub

        Private Function FindControl(ByVal controlName As String) As Control
            Return _MainLayoutControl.GetControlByName(controlName)
        End Function
    End Class
End Namespace
