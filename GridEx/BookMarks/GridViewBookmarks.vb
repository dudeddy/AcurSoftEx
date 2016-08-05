Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.Utils.Menu
Imports System.ComponentModel


Namespace AcurSoft.XtraGrid.Views.Grid.Bookmarks

    Public Class GridViewBookmarks
        Inherits Component
        Implements IDisposable
        <Flags>
        Public Enum ShowBookmarksModeEnum
            None = 1 << 2
            AllFlags = 1 << 3
            SelectedFlags = 1 << 4
            NotReady = 1 << 5
        End Enum

        Private _SelectedColors As List(Of Color)
        Public ReadOnly Property SelectedColors As List(Of Color)
            Get
                If _SelectedColors Is Nothing Then
                    _SelectedColors = New List(Of Color)
                End If
                Return _SelectedColors
            End Get
        End Property

        Public Function GetCanShowBookmarksMode(showBookmarksMode As ShowBookmarksModeEnum) As Boolean
            Return Me.CanShowBookmarksMode.HasFlag(showBookmarksMode)
        End Function

        Public ReadOnly Property CanShowBookmarksMode As ShowBookmarksModeEnum
            Get
                If Not Me.IsInitiated OrElse Me.BookMarks.Count = 0 Then
                    Return ShowBookmarksModeEnum.NotReady
                End If
                Select Case Me.ShowBookmarksMode
                    Case ShowBookmarksModeEnum.SelectedFlags
                        Return ShowBookmarksModeEnum.None Or ShowBookmarksModeEnum.SelectedFlags Or ShowBookmarksModeEnum.AllFlags
                    Case ShowBookmarksModeEnum.None
                        Return ShowBookmarksModeEnum.AllFlags Or ShowBookmarksModeEnum.SelectedFlags
                    Case ShowBookmarksModeEnum.AllFlags
                        Return ShowBookmarksModeEnum.None Or ShowBookmarksModeEnum.SelectedFlags
                End Select
                Return ShowBookmarksModeEnum.NotReady

            End Get
        End Property

        Public Function GetUsedColors() As List(Of Color)
            Return (From q In Me.BookMarks
                    Select c = q.Color Distinct).ToList()
        End Function


        Private _ShowBookmarksMode As ShowBookmarksModeEnum
        Public Property ShowBookmarksMode As ShowBookmarksModeEnum
            Get
                Return _ShowBookmarksMode
            End Get
            Set(value As ShowBookmarksModeEnum)
                'If _ShowBookmarksMode = value Then Return
                If Not Me.IsInitiated Then Return
                Me.View.BeginDataUpdate()
                RemoveHandler _View.CustomRowFilter, AddressOf GridView_CustomRowFilter
                Select Case value
                    Case ShowBookmarksModeEnum.None
                    Case ShowBookmarksModeEnum.AllFlags
                        AddHandler _View.CustomRowFilter, AddressOf GridView_CustomRowFilter
                    Case ShowBookmarksModeEnum.SelectedFlags
                        AddHandler _View.CustomRowFilter, AddressOf GridView_CustomRowFilter
                End Select

                _ShowBookmarksMode = value
                Me.View.EndDataUpdate()
            End Set
        End Property

        'Private _ShowBookmarkedOnly As Boolean
        'Public Property ShowBookmarkedOnly As Boolean
        '    Get
        '        Return _ShowBookmarkedOnly
        '    End Get
        '    Set(value As Boolean)
        '        If _ShowBookmarkedOnly = value Then Return
        '        If Not Me.IsInitiated Then Return
        '        Me.View.BeginDataUpdate()
        '        RemoveHandler _View.CustomRowFilter, AddressOf GridView_CustomRowFilter
        '        If value Then
        '            AddHandler _View.CustomRowFilter, AddressOf GridView_CustomRowFilter
        '        End If
        '        _ShowBookmarkedOnly = value
        '        Me.View.EndDataUpdate()
        '    End Set
        'End Property

        Private Sub GridView_CustomRowFilter(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowFilterEventArgs)
            'If Not _ShowBookmarkedOnly Then Return
            If   _ShowBookmarksMode = ShowBookmarksModeEnum.None Then Return
            Dim gv As GridViewEx = DirectCast(sender, GridViewEx)
            If Not gv.OptionsBehavior.BookmarksHelper.IsInitiated Then Return
            Dim v As Object = gv.GetListSourceRowCellValue(e.ListSourceRow, gv.OptionsBehavior.BookmarksHelper.KeyFieldName)
            Dim bmk As Bookmarks.BookmarkData = gv.OptionsBehavior.BookmarksHelper.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))
            Select Case Me._ShowBookmarksMode
                Case ShowBookmarksModeEnum.AllFlags
                    If bmk Is Nothing Then
                        e.Visible = False
                    Else
                        e.Visible = True
                    End If
                    e.Handled = True
                Case ShowBookmarksModeEnum.SelectedFlags
                    If bmk Is Nothing Then
                        e.Visible = False
                    ElseIf Not Me.SelectedColors.Contains(bmk.Color)
                        e.Visible = False
                    Else
                        e.Visible = True
                    End If
                    e.Handled = True

            End Select
        End Sub
        Public Property UseShortCuts As Boolean

        Private _View As GridView
        <Browsable(False)>
        Public ReadOnly Property View() As GridView
            Get
                Return _View
            End Get
        End Property

        Private _KeyFieldName As String
        Public Property KeyFieldName() As String
            Get
                Return _KeyFieldName
            End Get
            Set(value As String)
                If value Is _KeyFieldName Then Return
                Me.Init(value)
            End Set
        End Property

        Public ReadOnly Property IsInitiated As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me.KeyFieldName)
            End Get
        End Property

        Private _BookMarks As List(Of BookmarkData)
        <Browsable(False)>
        Public ReadOnly Property BookMarks() As List(Of BookmarkData)
            Get
                If _BookMarks Is Nothing Then
                    _BookMarks = New List(Of BookmarkData)
                End If
                Return _BookMarks
            End Get
        End Property

        Private _Palette As Dictionary(Of Color, Bitmap)
        Public Overridable ReadOnly Property Palette As Dictionary(Of Color, Bitmap)
            Get
                If _Palette Is Nothing Then
                    _Palette = New Dictionary(Of Color, Bitmap)
                    With _Palette
                        .Add(Color.Blue, Me.BitmapFromColor(Color.Blue))
                        .Add(Color.Navy, Me.BitmapFromColor(Color.Navy))
                        .Add(Color.Aqua, Me.BitmapFromColor(Color.Aqua))
                        .Add(Color.Teal, Me.BitmapFromColor(Color.Teal))
                        .Add(Color.Olive, Me.BitmapFromColor(Color.Olive))
                        .Add(Color.Green, Me.BitmapFromColor(Color.Green))
                        .Add(Color.Lime, Me.BitmapFromColor(Color.Lime))
                        .Add(Color.Yellow, Me.BitmapFromColor(Color.Yellow))
                        .Add(Color.Orange, Me.BitmapFromColor(Color.Orange))
                        .Add(Color.Red, Me.BitmapFromColor(Color.Red))
                        .Add(Color.Maroon, Me.BitmapFromColor(Color.Maroon))
                        .Add(Color.Fuchsia, Me.BitmapFromColor(Color.Fuchsia))
                        .Add(Color.Purple, Me.BitmapFromColor(Color.Purple))
                        .Add(Color.Gray, Me.BitmapFromColor(Color.Gray))
                    End With
                End If
                Return _Palette
            End Get
        End Property

        Public Function BitmapFromColor(color As Color) As Bitmap
            Dim colorBmp As New Bitmap(14, 14)
            Using gfx As Graphics = Graphics.FromImage(colorBmp)
                Using brush As New SolidBrush(color)
                    gfx.FillRectangle(brush, 0, 0, 14, 14)
                End Using
            End Using
            Return colorBmp
        End Function

        Public Sub New(ByVal view As GridView)
            _View = view
            _UseShortCuts = True
            _ShowBookmarksMode = ShowBookmarksModeEnum.None
        End Sub


        Public Sub Init(ByVal keyFieldName As String)
            _KeyFieldName = keyFieldName
            RemoveHandler Me.View.PopupMenuShowing, AddressOf View_PopupMenuShowing
            AddHandler Me.View.PopupMenuShowing, AddressOf View_PopupMenuShowing

            RemoveHandler View.KeyDown, AddressOf View_KeyDown
            AddHandler Me.View.KeyDown, AddressOf View_KeyDown

            RemoveHandler View.CustomDrawRowIndicator, AddressOf View_CustomDrawRowIndicator
            AddHandler Me.View.CustomDrawRowIndicator, AddressOf View_CustomDrawRowIndicator
        End Sub

        Public Overridable Function GetBookmarkIndexToAdd() As Integer
            Dim index As Integer = 0
            If Me.BookMarks.Count > 0 Then
                Dim bookMarkedMax As Integer = Me.BookMarks.Max(Function(q) q.Index)
                Dim indexes As List(Of Integer) = Me.BookMarks.Select(Function(q) q.Index).ToList
                Dim missingIndexes As List(Of Integer) = Enumerable.Range(0, bookMarkedMax).Except(indexes).ToList
                If missingIndexes.Count = 0 Then
                    index = Me.BookMarks.Count
                Else
                    index = missingIndexes.First
                End If
            End If
            Return index
        End Function

        Public Overridable Function GetBookmarkCaption(bmk As BookmarkData) As String
            Return String.Format(String.Format("Bookmark : [{0}]", bmk.Index + 1))
        End Function

        Public Overridable Function GetAddBookmarkCaption(index As Integer, Optional focused As Boolean = False) As String
            Dim caption As String = Nothing
            If focused Then
                caption = "Add Focuced Row to bookmark : [{0}]"
            Else
                caption = "Add to bookmark : [{0}]"
            End If
            Return String.Format(caption, index + 1)
        End Function
        Public Overridable Function GetRemoveBookmarkCaption(bmk As BookmarkData, Optional focused As Boolean = False) As String
            Dim caption As String = Nothing
            If focused Then
                caption = "Remove Focuced Row from bookmarks : [{0}]"
            Else
                caption = "Remove from bookmarks : [{0}]"
            End If
            Return String.Format(caption, bmk.Index + 1)
        End Function

        Public Overridable Function AddToBookmark(v As Object, index As Integer, rowHandle As Integer) As BookmarkData
            Dim bmk As New BookmarkData(v, index)
            Me.BookMarks.Add(bmk)
            View.InvalidateRowIndicator(rowHandle)
            Me.UpdateButtons()
            Return bmk
        End Function
        Public Overridable Function AddToBookmark(v As Object, index As Integer, rowHandle As Integer, color As Color) As BookmarkData
            Dim bmk As New BookmarkData(v, index) With {.Color = color}
            Me.BookMarks.Add(bmk)
            View.InvalidateRowIndicator(rowHandle)
            Me.UpdateButtons()
            Return bmk
        End Function

        Public Overridable Sub RemoveFromBookmarks(bmk As BookmarkData, rowHandle As Integer)
            Me.BookMarks.Remove(bmk)
            View.InvalidateRowIndicator(rowHandle)
            Me.UpdateButtons()
        End Sub

        Public Overridable Function GotoBookmark(bmk As BookmarkData) As Integer
            Dim rh As Integer = Me.View.DataController.FindRowByValue(Me.KeyFieldName, bmk.Value)
            View.FocusedRowHandle = rh
            Return rh
        End Function

        Public Overridable Function GetBookmarkData(rh As Integer) As BookmarkData
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim v As Object = Me.View.GetRowCellValue(rh, Me.KeyFieldName)
            Dim bookMarkedCount As Integer = Me.BookMarks.Count
            Return Me.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))

        End Function

        'Public Overridable Function CreateAddBookmarkMenuItem(rh As Integer) As DXMenuItem
        '    If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
        '    Dim bookMarkedCount As Integer = Me.BookMarks.Count
        '    Dim bmk As BookmarkData = Me.GetBookmarkData(rh)
        '    Dim focused As Boolean = rh = Me.View.FocusedRowHandle
        '    If bmk Is Nothing Then
        '        Dim index As Integer = Me.GetBookmarkIndexToAdd()
        '        Dim itemAdd As New DXMenuItem(Me.GetAddBookmarkCaption(index, focused))
        '        itemAdd.Image = Ressources.bookmark_add_16x16
        '        AddHandler itemAdd.Click,
        '            Sub(s, a)
        '                Me.AddToBookmark(Me.View.GetRowCellValue(rh, Me.KeyFieldName), index, rh)
        '            End Sub
        '        Return itemAdd
        '    End If
        '    'Dim si As DXSubMenuItem
        '    'AddHandler si.
        '    Return Nothing
        'End Function


        Public Overridable Function CreateAddBookmarkSubMenuItem(rh As Integer) As DXSubMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim bookMarkedCount As Integer = Me.BookMarks.Count
            Dim bmk As BookmarkData = Me.GetBookmarkData(rh)
            Dim focused As Boolean = rh = Me.View.FocusedRowHandle
            If bmk Is Nothing Then
                Dim index As Integer = Me.GetBookmarkIndexToAdd()
                Dim itemAdd As New DXSubMenuItem(Me.GetAddBookmarkCaption(index, focused))
                itemAdd.Image = Ressources.bookmark_add_16x16
                For Each p In Me.Palette
                    Dim mi As New DXMenuItem(p.Key.ToString()) With {.Image = p.Value}
                    AddHandler mi.Click,
                        Sub(s, a)
                            Me.AddToBookmark(Me.View.GetRowCellValue(rh, Me.KeyFieldName), index, rh, p.Key)
                        End Sub
                    itemAdd.Items.Add(mi)
                Next
                Return itemAdd
            End If
            'Dim si As DXSubMenuItem
            'AddHandler si.
            Return Nothing
        End Function


        Public Overridable Function CreateRemoveBookmarkMenuItem(rh As Integer) As DXMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            'Dim v As Object = Me.View.GetRowCellValue(rh, Me.KeyFieldName)
            'Dim bookMarkedCount As Integer = Me.BookMarks.Count
            Dim bmk As BookmarkData = Me.GetBookmarkData(rh)
            Dim focused As Boolean = rh = Me.View.FocusedRowHandle
            If bmk Is Nothing Then
                Return Nothing
            Else
                Dim removeItem As New DXMenuItem(Me.GetRemoveBookmarkCaption(bmk, focused))
                removeItem.Image = Ressources.bookmark_remove_16x16
                AddHandler removeItem.Click,
                Sub(s, a)
                    Me.RemoveFromBookmarks(bmk, rh)
                End Sub
                Return removeItem
            End If
        End Function

        Public Overridable Function CreateClearBookmarksMenuItem() As DXMenuItem
            If Me.BookMarks.Count = 0 Then Return Nothing
            Dim clearItem As New DXMenuItem("Clear Bookmarks")
            clearItem.Image = Ressources.bookmark_delete_16x16

            AddHandler clearItem.Click,
            Sub(s, a)
                If DevExpress.XtraEditors.XtraMessageBox.Show("Clear Bookmarks?", "Clear Bookmarks", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                    Me.BookMarks.Clear()
                    Me.View.BeginDataUpdate()
                    Me.ShowBookmarksMode = ShowBookmarksModeEnum.None
                    Me.View.EndDataUpdate()
                    Me.View.Invalidate()
                    Me.UpdateButtons()
                End If
            End Sub
            Return clearItem
        End Function

        Public Sub SetShowBookmarksMode(showBookmarksMode As ShowBookmarksModeEnum)
            Me.ShowBookmarksMode = showBookmarksMode
            Me.UpdateButtons()
        End Sub

        Private Sub UpdateButtons()
            Dim gv As GridViewEx = TryCast(_View, GridViewEx)
            If gv Is Nothing Then Return
            If Me.IsInitiated Then
                gv.FindControl.ButtonBookmarkShowAllRows.Enabled = Me.GetCanShowBookmarksMode(ShowBookmarksModeEnum.None)
                gv.FindControl.ButtonBookmarkShowFiltredOnly.Enabled = Me.GetCanShowBookmarksMode(ShowBookmarksModeEnum.AllFlags)
                gv.FindControl.ButtonBookmarkShowSelected.Enabled = Me.GetUsedColors.Count > 1
            Else
                gv.FindControl.ButtonBookmarkShowAllRows.Enabled = False
                gv.FindControl.ButtonBookmarkShowFiltredOnly.Enabled = False
                gv.FindControl.ButtonBookmarkShowSelected.Enabled = False
            End If
        End Sub

        Public Overridable Sub CreateFilterBookmarksMenuItems(items As DXMenuItemCollection, Optional beginGroup As Boolean = False)
            Dim showAllRowsItem As New DXMenuCheckItem("Show All Rows") With {
                .BeginGroup = beginGroup,
                .Image = Ressources.bookmark_filter_deabled_16x16,
                .Checked = Me.ShowBookmarksMode = GridViewBookmarks.ShowBookmarksModeEnum.None}
            showAllRowsItem.Visible = Not showAllRowsItem.Checked
            AddHandler showAllRowsItem.CheckedChanged,
                Sub(s, a)
                    Me.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.None)
                    'Me.ShowBookmarksMode = GridViewBookmarks.ShowBookmarksModeEnum.None
                End Sub
            items.Add(showAllRowsItem)

            Dim showBookmarksOnlyItem As New DXMenuCheckItem("Show Bookmarks Only") With {
                .BeginGroup = Not showAllRowsItem.Visible,
                .Image = Ressources.bookmark_filter_16x16,
                .Checked = Me.ShowBookmarksMode = GridViewBookmarks.ShowBookmarksModeEnum.AllFlags}
            showBookmarksOnlyItem.Visible = Not showBookmarksOnlyItem.Checked

            AddHandler showBookmarksOnlyItem.CheckedChanged,
                Sub(s, a)
                    'Me.ShowBookmarksMode = GridViewBookmarks.ShowBookmarksModeEnum.AllFlags
                    Me.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.AllFlags)
                End Sub
            items.Add(showBookmarksOnlyItem)

            Dim mi As New DXMenuCheckItem("Show Selected Flags") With {
                .Checked = Me.ShowBookmarksMode = GridViewBookmarks.ShowBookmarksModeEnum.SelectedFlags,
                .Enabled = Me.GetUsedColors.Count > 1,
                .Image = Ressources.bookmark_filter_select_16x16}
            AddHandler mi.Click, Sub(s, a)
                                     Using frm As New GridBookmarksColorsForm(Me)
                                         If frm.ShowDialog() = DialogResult.OK Then
                                             'Me.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.SelectedFlags)
                                         End If
                                     End Using
                                 End Sub
            items.Add(mi)
        End Sub

        Public Function SelectFlags() As Boolean
            Dim b As Boolean = False
            Using frm As New GridBookmarksColorsForm(Me)
                If frm.ShowDialog() = DialogResult.OK Then
                    b = False
                End If
            End Using
            Return b
        End Function


        Public Overridable Sub CreateGotoBookmarksMenuItems(items As DXMenuItemCollection, Optional beginGroup As Boolean = False)
            For Each b In Me.BookMarks
                'Dim enabled As Boolean = Not skip.Invoke(b)
                'Dim enabled As Boolean = True
                Dim img As Bitmap = Me.Palette(b.Color)
                Dim item As New DXMenuItem(Me.GetBookmarkCaption(b)) With {
                .Image = img,
                .BeginGroup = beginGroup}
                If Me.ShowBookmarksMode = ShowBookmarksModeEnum.SelectedFlags Then
                    item.Enabled = Me.SelectedColors.Contains(b.Color)
                End If
                AddHandler item.Click,
                                    Sub(s, a)
                                        Me.GotoBookmark(b)
                                    End Sub
                If b.Value.Equals(Me.View.GetFocusedRowCellValue(Me.KeyFieldName)) Then
                    item.Appearance.Options.UseForeColor = True
                    item.Appearance.ForeColor = Color.Blue
                End If

                items.Add(item)
                beginGroup = False
            Next
        End Sub


        Public Overridable Function CreateSetBookmarkColorMenuItem(rh As Integer) As DXSubMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim bmk As BookmarkData = Me.GetBookmarkData(rh)
            If bmk Is Nothing Then Return Nothing
            Dim colorsSubItems As New DXSubMenuItem("Set Flag to:")

            For Each clr In Me.Palette
                Dim colorItem As New DXMenuCheckItem(clr.Key.Name) With {.Image = clr.Value}
                If bmk.Color = clr.Key Then
                    colorItem.Checked = True
                End If

                AddHandler colorItem.Click,
                Sub(s, a)
                    _View.BeginDataUpdate()
                    If Me.ShowBookmarksMode = ShowBookmarksModeEnum.SelectedFlags Then
                        If Me.SelectedColors.LongCount(Function(q) q.Equals(clr.Key)) = 1 Then
                            Me.SelectedColors.Remove(clr.Key)
                        End If
                        If Not Me.SelectedColors.Any(Function(q) q.Equals(clr.Key)) Then
                            Me.SelectedColors.Add(clr.Key)
                        End If
                    End If
                    bmk.Color = clr.Key
                    _View.EndDataUpdate()
                    View.InvalidateRowIndicator(rh)
                End Sub

                colorsSubItems.Items.Add(colorItem)
            Next
            Return colorsSubItems

        End Function

        Public Overridable Sub View_PopupMenuShowing(sender As Object, e As PopupMenuShowingEventArgs)
            If e.HitInfo.HitTest <> GridHitTest.RowIndicator Then Return
            Dim rh As Integer = e.HitInfo.RowHandle
            If Not Me.View.IsValidRowHandle(rh) Then Return
            Dim v As Object = Me.View.GetRowCellValue(rh, Me.KeyFieldName)
            Dim bookMarkedCount As Integer = Me.BookMarks.Count
            Dim bmk As BookmarkData = Me.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))
            'Dim itemAdd As DXMenuItem = Me.CreateAddBookmarkMenuItem(rh)
            'If itemAdd IsNot Nothing Then
            '    e.Menu.Items.Add(itemAdd)
            'End If
            Dim itemAdd As DXSubMenuItem = Me.CreateAddBookmarkSubMenuItem(rh)
            If itemAdd IsNot Nothing Then
                e.Menu.Items.Add(itemAdd)
            End If
            Dim removeItem As DXMenuItem = Me.CreateRemoveBookmarkMenuItem(rh)
            If removeItem IsNot Nothing Then
                removeItem.BeginGroup = itemAdd IsNot Nothing
                e.Menu.Items.Add(removeItem)
                Dim colorsSubItems As DXSubMenuItem = Me.CreateSetBookmarkColorMenuItem(rh)
                If colorsSubItems IsNot Nothing Then
                    colorsSubItems.BeginGroup = True
                    e.Menu.Items.Add(colorsSubItems)
                End If
            End If

            If bookMarkedCount = 0 Then Return
            If Not (bookMarkedCount = 1 AndAlso Me.BookMarks.FirstOrDefault Is bmk) Then
                Dim gotoSubItems As New DXSubMenuItem("Go to bookmark") With {.BeginGroup = True}
                Me.CreateGotoBookmarksMenuItems(gotoSubItems.Items)
                e.Menu.Items.Add(gotoSubItems)
            End If

            Dim clearItem As DXMenuItem = Me.CreateClearBookmarksMenuItem()
            If clearItem IsNot Nothing Then
                clearItem.BeginGroup = True
                e.Menu.Items.Add(clearItem)
                Me.CreateFilterBookmarksMenuItems(e.Menu.Items, True)
            End If

        End Sub


        Protected Overridable Sub DrawBookmark(ByVal e As RowIndicatorCustomDrawEventArgs)
            Dim v As Object = Me.View.GetRowCellValue(e.RowHandle, Me.KeyFieldName)
            Dim bkm As BookmarkData = Me.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))
            If bkm Is Nothing Then Return

            Using brush As New SolidBrush(bkm.Color)
                Dim pen As New Pen(bkm.Color, 4)
                'e.Graphics.DrawRectangle(pen, New Rectangle(e.Info.Bounds.X + 1, e.Info.Bounds.Y + 1, e.Info.Bounds.Width - 3, e.Info.Bounds.Height - 3))
                e.Graphics.DrawRectangle(pen, New Rectangle(e.Info.Bounds.X + 1, e.Info.Bounds.Y + e.Info.Bounds.Height - 4, e.Info.Bounds.Width - 4, 4))
                'Dim bounds As New RectangleF()
                'bounds.X = e.Info.Bounds.X
                'bounds.Y = e.Info.Bounds.Y
                'bounds.Width = e.Info.Bounds.Width
                'bounds.Height = e.Info.Bounds.Height '/ 2
                'e.Graphics.FillRectangle(brush, bounds)
            End Using
        End Sub

        Private Sub View_CustomDrawRowIndicator(ByVal sender As Object, ByVal e As RowIndicatorCustomDrawEventArgs)
            If Not e.Info.IsRowIndicator Then Return
            Dim v As Object = Me.View.GetRowCellValue(e.RowHandle, Me.KeyFieldName)
            If Me.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v)) Is Nothing Then Return
            e.Painter.DrawObject(e.Info) ' Default drawing
            e.Handled = True
            DrawBookmark(e)
        End Sub

        Public Overridable Sub View_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If Not e.Control Then Return
            If Not (e.KeyCode <= Keys.D0 OrElse e.KeyCode > Keys.D9) Then
                Dim index As Integer = e.KeyCode - Keys.D0 - 1
                Dim bmk As BookmarkData = Me.BookMarks.FirstOrDefault(Function(q) q.Index = index)
                If bmk IsNot Nothing Then
                    Me.GotoBookmark(bmk)
                End If
            ElseIf e.KeyCode = Keys.D
                If Me.View.OptionsBehavior.Editable Then Return
                Dim v As Object = Me.View.GetFocusedRowCellValue(Me.KeyFieldName)
                Dim bmk As BookmarkData = Me.BookMarks.FirstOrDefault(Function(q) q.Value.Equals(v))
                If bmk Is Nothing Then
                    Me.AddToBookmark(v, Me.GetBookmarkIndexToAdd(), Me.View.FocusedRowHandle)
                Else
                    Me.RemoveFromBookmarks(bmk, Me.View.FocusedRowHandle)
                End If

            End If
        End Sub


#Region "IDisposable Members"
        Public Shadows Sub Dispose() Implements IDisposable.Dispose
            If View IsNot Nothing AndAlso (Not View.GridControl.IsDisposed) Then
                RemoveHandler View.KeyDown, AddressOf View_KeyDown
                RemoveHandler View.PopupMenuShowing, AddressOf View_PopupMenuShowing
                RemoveHandler View.CustomDrawRowIndicator, AddressOf View_CustomDrawRowIndicator
                _View = Nothing
            End If
        End Sub
#End Region
    End Class
End Namespace
