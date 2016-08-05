Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.Utils.Menu
Imports System.ComponentModel



Namespace AcurSoft.XtraGrid.Views.Grid.GridFlags


    Public Class GridViewFlags
        Inherits Component
        Implements IDisposable

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

        Private _Flags As List(Of GridFlagData)
        <Browsable(False)>
        Public ReadOnly Property Flags() As List(Of GridFlagData)
            Get
                If _Flags Is Nothing Then
                    _Flags = New List(Of GridFlagData)
                End If
                Return _Flags
            End Get
        End Property

        Private _Palette As Dictionary(Of Color, Bitmap)
        Public Overridable ReadOnly Property Palette As Dictionary(Of Color, Bitmap)
            Get
                If _Palette Is Nothing Then
                    _Palette = New Dictionary(Of Color, Bitmap)
                    With _Palette
                        .Add(Color.Navy, Me.BitmapFromColor(Color.Navy))
                        .Add(Color.Blue, Me.BitmapFromColor(Color.Blue))
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

        Public Overridable Function GetFlagIndexToAdd() As Integer
            Dim index As Integer = 0
            If Me.Flags.Count > 0 Then
                Dim bookMarkedMax As Integer = Me.Flags.Max(Function(q) q.Index)
                Dim indexes As List(Of Integer) = Me.Flags.Select(Function(q) q.Index).ToList
                Dim missingIndexes As List(Of Integer) = Enumerable.Range(0, bookMarkedMax).Except(indexes).ToList
                If missingIndexes.Count = 0 Then
                    index = Me.Flags.Count
                Else
                    index = missingIndexes.First
                End If
            End If
            Return index
        End Function

        Public Overridable Function GetFlagCaption(bmk As GridFlagData) As String
            Return String.Format(String.Format("Bookmark : [{0}]", bmk.Index + 1))
        End Function

        Public Overridable Function GetAddFlagCaption(index As Integer, Optional focused As Boolean = False) As String
            Dim caption As String = Nothing
            If focused Then
                caption = "Add Focuced Row to bookmark : [{0}]"
            Else
                caption = "Add to bookmark : [{0}]"
            End If
            Return String.Format(caption, index + 1)
        End Function
        Public Overridable Function GetRemoveFlagCaption(bmk As GridFlagData, Optional focused As Boolean = False) As String
            Dim caption As String = Nothing
            If focused Then
                caption = "Remove Focuced Row from Flags : [{0}]"
            Else
                caption = "Remove from flags : [{0}]"
            End If
            Return String.Format(caption, bmk.Index + 1)
        End Function

        Public Overridable Function AddToFlag(v As Object, index As Integer, rowHandle As Integer) As GridFlagData
            Dim bmk As New GridFlagData(v, index)
            Me.Flags.Add(bmk)
            View.InvalidateRowIndicator(rowHandle)
            Return bmk
        End Function

        Public Overridable Sub RemoveFromFlags(bmk As GridFlagData, rowHandle As Integer)
            Me.Flags.Remove(bmk)
            View.InvalidateRowIndicator(rowHandle)
        End Sub

        Public Overridable Function GotoFlag(bmk As GridFlagData) As Integer
            Dim rh As Integer = Me.View.DataController.FindRowByValue(Me.KeyFieldName, bmk.Value)
            View.FocusedRowHandle = rh
            Return rh
        End Function

        Public Overridable Function GetFlagData(rh As Integer) As GridFlagData
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim v As Object = Me.View.GetRowCellValue(rh, Me.KeyFieldName)
            Dim bookMarkedCount As Integer = Me.Flags.Count
            Return Me.Flags.FirstOrDefault(Function(q) q.Value.Equals(v))
        End Function

        Public Overridable Function CreateAddFlagMenuItem(rh As Integer) As DXMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            'Dim v As Object = Me.View.GetRowCellValue(rh, Me.KeyFieldName)
            Dim bookMarkedCount As Integer = Me.Flags.Count
            Dim bmk As GridFlagData = Me.GetFlagData(rh)
            Dim focused As Boolean = rh = Me.View.FocusedRowHandle
            If bmk Is Nothing Then
                Dim index As Integer = Me.GetFlagIndexToAdd()
                Dim itemAdd As New DXMenuItem(Me.GetAddFlagCaption(index, focused))
                itemAdd.Image = Ressources.bookmark_add_16x16
                AddHandler itemAdd.Click,
                Sub(s, a)
                    Me.AddToFlag(Me.View.GetRowCellValue(rh, Me.KeyFieldName), index, rh)
                End Sub
                Return itemAdd
            End If
            Return Nothing
        End Function

        Public Overridable Function CreateRemoveFlagMenuItem(rh As Integer) As DXMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim bmk As GridFlagData = Me.GetFlagData(rh)
            Dim focused As Boolean = rh = Me.View.FocusedRowHandle
            If bmk Is Nothing Then
                Return Nothing
            Else
                Dim removeItem As New DXMenuItem(Me.GetRemoveFlagCaption(bmk, focused))
                removeItem.Image = Ressources.bookmark_remove_16x16
                AddHandler removeItem.Click,
                Sub(s, a)
                    Me.RemoveFromFlags(bmk, rh)
                End Sub
                Return removeItem
            End If
        End Function

        Public Overridable Function CreateClearFlagsMenuItem() As DXMenuItem
            If Me.Flags.Count = 0 Then Return Nothing
            Dim clearItem As New DXMenuItem("Clear Flags")
            clearItem.Image = Ressources.bookmark_delete_16x16

            AddHandler clearItem.Click,
            Sub(s, a)
                If DevExpress.XtraEditors.XtraMessageBox.Show("Clear Flags?", "Clear Flags", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
                    Me.Flags.Clear()
                    Me.View.Invalidate()
                End If
            End Sub
            Return clearItem
        End Function
        Public Overridable Sub CreateGotoFlagsMenuItems(items As DXMenuItemCollection, skip As Func(Of GridFlagData, Boolean), Optional beginGroup As Boolean = False)
            For Each b In Me.Flags
                Dim enabled As Boolean = Not skip.Invoke(b)
                Dim img As Bitmap = Me.Palette(b.Color)
                Dim item As New DXMenuItem(Me.GetFlagCaption(b)) With {
                .Image = img,
                .Enabled = enabled,
                .BeginGroup = beginGroup}
                AddHandler item.Click,
                Sub(s, a)
                    Me.GotoFlag(b)
                End Sub
                items.Add(item)
                beginGroup = False
            Next
        End Sub


        Public Overridable Function CreateSetFlagColorMenuItem(rh As Integer) As DXSubMenuItem
            If Not Me.View.IsValidRowHandle(rh) Then Return Nothing
            Dim bmk As GridFlagData = Me.GetFlagData(rh)
            If bmk Is Nothing Then Return Nothing
            Dim colorsSubItems As New DXSubMenuItem("Set Color to:")

            For Each clr In Me.Palette
                Dim colorItem As New DXMenuCheckItem(clr.Key.Name) With {.Image = clr.Value}
                If bmk.Color = clr.Key Then
                    colorItem.Checked = True
                End If

                AddHandler colorItem.Click,
                Sub(s, a)
                    bmk.Color = clr.Key
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
            Dim bookMarkedCount As Integer = Me.Flags.Count
            Dim bmk As GridFlagData = Me.Flags.FirstOrDefault(Function(q) q.Value.Equals(v))
            Dim itemAdd As DXMenuItem = Me.CreateAddFlagMenuItem(rh)
            If itemAdd IsNot Nothing Then
                e.Menu.Items.Add(itemAdd)
            End If
            Dim removeItem As DXMenuItem = Me.CreateRemoveFlagMenuItem(rh)
            If removeItem IsNot Nothing Then
                removeItem.BeginGroup = itemAdd IsNot Nothing
                e.Menu.Items.Add(removeItem)
                Dim colorsSubItems As DXSubMenuItem = Me.CreateSetFlagColorMenuItem(rh)
                If colorsSubItems IsNot Nothing Then
                    colorsSubItems.BeginGroup = True
                    e.Menu.Items.Add(colorsSubItems)
                End If
            End If
            'If bmk Is Nothing Then
            'Else
            '    'Dim removeItem As New DXMenuItem(Me.GetRemoveBookmarkCaption(bmk))

            'End If
            If bookMarkedCount = 0 Then Return
            If bookMarkedCount = 1 AndAlso Me.Flags.FirstOrDefault Is bmk Then Return

            Dim gotoSubItems As New DXSubMenuItem("Go to bookmark") With {.BeginGroup = True}
            Me.CreateGotoFlagsMenuItems(gotoSubItems.Items, Function(q)
                                                                Return q.Value.Equals(Me.View.GetFocusedRowCellValue(Me.KeyFieldName))
                                                            End Function)


            e.Menu.Items.Add(gotoSubItems)

            Dim clearItem As DXMenuItem = Me.CreateClearFlagsMenuItem()
            If clearItem IsNot Nothing Then
                clearItem.BeginGroup = True
                e.Menu.Items.Add(clearItem)
            End If

        End Sub


        Protected Overridable Sub DrawFlag(ByVal e As RowIndicatorCustomDrawEventArgs)
            Dim v As Object = Me.View.GetRowCellValue(e.RowHandle, Me.KeyFieldName)
            Dim bkm As GridFlagData = Me.Flags.FirstOrDefault(Function(q) q.Value.Equals(v))
            If bkm Is Nothing Then Return

            Using brush As New SolidBrush(bkm.Color)
                Dim pen As New Pen(bkm.Color, 4)
                e.Graphics.DrawRectangle(pen, New Rectangle(e.Info.Bounds.X + 1, e.Info.Bounds.Y + e.Info.Bounds.Height - 4, e.Info.Bounds.Width - 4, 4))
            End Using
        End Sub

        Private Sub View_CustomDrawRowIndicator(ByVal sender As Object, ByVal e As RowIndicatorCustomDrawEventArgs)
            If Not e.Info.IsRowIndicator Then Return
            Dim v As Object = Me.View.GetRowCellValue(e.RowHandle, Me.KeyFieldName)
            If Me.Flags.FirstOrDefault(Function(q) q.Value.Equals(v)) Is Nothing Then Return
            e.Painter.DrawObject(e.Info) ' Default drawing
            e.Handled = True
            DrawFlag(e)
        End Sub

        Public Overridable Sub View_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If Not e.Control Then Return
            If Not (e.KeyCode <= Keys.D0 OrElse e.KeyCode > Keys.D9) Then
                Dim index As Integer = e.KeyCode - Keys.D0 - 1
                Dim bmk As GridFlagData = Me.Flags.FirstOrDefault(Function(q) q.Index = index)
                If bmk IsNot Nothing Then
                    Me.GotoFlag(bmk)
                End If
            ElseIf e.KeyCode = Keys.D
                If Me.View.OptionsBehavior.Editable Then Return
                Dim v As Object = Me.View.GetFocusedRowCellValue(Me.KeyFieldName)
                Dim bmk As GridFlagData = Me.Flags.FirstOrDefault(Function(q) q.Value.Equals(v))
                If bmk Is Nothing Then
                    Me.AddToFlag(v, Me.GetFlagIndexToAdd(), Me.View.FocusedRowHandle)
                Else
                    Me.RemoveFromFlags(bmk, Me.View.FocusedRowHandle)
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
