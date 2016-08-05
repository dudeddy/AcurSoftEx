Namespace AcurSoft.XtraGrid.Views.Grid.Bookmarks


    Public Class GridBookmarksColorsForm



        Public Sub New()
            InitializeComponent()
        End Sub


        Public ReadOnly Property BookmarkHelper As GridViewBookmarks
        Private ReadOnly Property GridData As List(Of GridDataElement)

        Private _SelectedColors As List(Of Color)
        Private ReadOnly Property SelectedColors As List(Of Color)
            Get
                If _SelectedColors Is Nothing Then
                    _SelectedColors = New List(Of Color)
                End If
                Return _SelectedColors
            End Get
        End Property

        Private Class GridDataElement
            Public Property Color As Color
            Public Property Display As String
        End Class

        Public Sub New(bkm As GridViewBookmarks)
            Me.New()
            _BookmarkHelper = bkm
            _GridData = (From q In bkm.GetUsedColors
                         Select New GridDataElement With {
                             .Color = q,
                             .Display = q.ToString.Replace("Color ", "").Trim("["c).Trim("]"c)
                             }).ToList()

            Me.GridControl.DataSource = _GridData
            Select Case bkm.ShowBookmarksMode
                Case GridViewBookmarks.ShowBookmarksModeEnum.None, GridViewBookmarks.ShowBookmarksModeEnum.AllFlags
                    Me.GridView.SelectAll()
                Case GridViewBookmarks.ShowBookmarksModeEnum.SelectedFlags
                    For i As Integer = 0 To Me.GridView.RowCount - 1
                        Dim color As Color = DirectCast(Me.GridView.GetRowCellValue(i, "Color"), Color)
                        If bkm.SelectedColors.Any(Function(q) q.Equals(color)) Then
                            Me.GridView.SelectRow(i)
                        End If
                    Next
            End Select
        End Sub

        Private Sub GridView_CustomDrawCell(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridView.CustomDrawCell
            Select Case e.Column.FieldName
                Case "Color"
                    e.Appearance.Options.UseBackColor = True
                    e.Appearance.BackColor = DirectCast(e.CellValue, Color)
                    'e.Handled = True
            End Select
        End Sub

        Private Sub btn_ok_Click(sender As Object, e As EventArgs) Handles btn_ok.Click
            Me.BookmarkHelper.SelectedColors.Clear()
            For i As Integer = 0 To Me.GridView.RowCount
                If Me.GridView.IsRowSelected(i) Then
                    Me.BookmarkHelper.SelectedColors.Add(DirectCast(Me.GridView.GetRowCellValue(i, "Color"), Color))
                End If
            Next
            Me.BookmarkHelper.SetShowBookmarksMode(GridViewBookmarks.ShowBookmarksModeEnum.SelectedFlags)
            Me.DialogResult = DialogResult.OK
        End Sub

        Private Sub btn_cancel_Click(sender As Object, e As EventArgs) Handles btn_cancel.Click
            Me.DialogResult = DialogResult.Cancel
        End Sub

        Private Sub GridView_SelectionChanged(sender As Object, e As DevExpress.Data.SelectionChangedEventArgs) Handles GridView.SelectionChanged
            Me.btn_ok.Enabled = Me.GridView.SelectedRowsCount > 0
        End Sub

        Private Sub GridView_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView.CustomColumnDisplayText
            Select Case e.Column.FieldName
                Case "Color"
                    e.DisplayText = ""
            End Select

        End Sub
    End Class
End Namespace
