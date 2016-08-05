Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Linq
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Grid.Extenders


    Public Class GridSortForm
        Private _Data As BindingList(Of SortElement)

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _Data = New BindingList(Of SortElement)

            Dim dicSortOn As New Dictionary(Of DevExpress.XtraGrid.ColumnSortMode, String)
            dicSortOn.Add(DevExpress.XtraGrid.ColumnSortMode.Default, "Default")
            dicSortOn.Add(DevExpress.XtraGrid.ColumnSortMode.DisplayText, "DisplayText")
            dicSortOn.Add(DevExpress.XtraGrid.ColumnSortMode.Value, "Value")

            Me.redSortOn.DataSource = dicSortOn
            Me.redSortOn.ShowFooter = False
            Me.redSortOn.ShowHeader = False


            Dim dicSortOrder As New Dictionary(Of DevExpress.Data.ColumnSortOrder, String)
            dicSortOrder.Add(DevExpress.Data.ColumnSortOrder.Ascending, "Ascending")
            dicSortOrder.Add(DevExpress.Data.ColumnSortOrder.Descending, "Descending")
            Me.redSortOrder.DataSource = dicSortOrder
            Me.redSortOrder.ShowFooter = False
            Me.redSortOrder.ShowHeader = False

            Me.GridControl.DataSource = _Data
        End Sub

        Public ReadOnly Property View As GridView
        Private _Columns As DataTable
        Public Sub New(view As GridView)
            Me.New
            _View = view
            _Columns = New DataTable
            _Columns.Columns.Add("field")
            _Columns.Columns.Add("caption")

            For Each col As GridColumn In view.Columns.OfType(Of GridColumn)
                If col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False Then
                    Continue For
                End If
                Dim nr As DataRow = _Columns.NewRow
                nr.Item(0) = col.FieldName
                nr.Item(1) = col.GetCaption()
                _Columns.Rows.Add(nr)
            Next
            _Columns.AcceptChanges()
            Me.redColumn.DataSource = _Columns
            redColumn.DisplayMember = "caption"
            redColumn.ValueMember = "field"
            redColumn.PopulateColumns()
            redColumn.Columns.Item(0).Visible = False
            Me.redColumn.ShowFooter = False
            Me.redColumn.ShowHeader = False
            Dim index As Integer = 0
            For Each col As GridColumn In Me.View.SortedColumns.OfType(Of GridColumn).OrderBy(Function(q) q.SortIndex)
                _Data.Add(New SortElement With {.Col = col.FieldName, .Direction = col.SortOrder, .SortOn = col.SortMode, .Order = index})
                index += 1
            Next
        End Sub

        Private Sub btnUp_Click(sender As Object, e As System.EventArgs) Handles btnUp.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            Dim prevIndex As Integer = index - 1
            If prevIndex = -1 Then Return
            Me.GridView.BeginDataUpdate()

            Dim prevElm As SortElement = _Data.FirstOrDefault(Function(q) q.Order = prevIndex)
            Dim Elm As SortElement = _Data.FirstOrDefault(Function(q) q.Order = index)

            prevElm.Order = index
            Elm.Order = prevIndex
            Me.GridView.EndDataUpdate()
        End Sub


        Private Sub btnDown_Click(sender As Object, e As System.EventArgs) Handles btnDown.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            If index = _Data.Max(Function(q) q.Order) Then Return
            Me.GridView.BeginDataUpdate()
            Dim nextIndex As Integer = index + 1

            Dim nextElm As SortElement = _Data.FirstOrDefault(Function(q) q.Order = nextIndex)
            Dim Elm As SortElement = _Data.FirstOrDefault(Function(q) q.Order = index)

            nextElm.Order = index
            Elm.Order = nextIndex
            Me.GridView.EndDataUpdate()

        End Sub


        Private Sub GridView_CustomRowCellEditForEditing(sender As Object, e As DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs) Handles GridView.CustomRowCellEditForEditing
            'If Me.GridView.IsNewItemRow(e.RowHandle) Then
            If e.Column Is Nothing Then Return
            If e.Column.FieldName = "Col" Then
                Dim red As New RepositoryItemLookUpEdit

                red.Assign(Me.redColumn)

                Dim dt As DataTable = _Columns.Clone
                For i As Integer = 0 To _Columns.Rows.Count - 1
                    Dim r As DataRow = _Columns.Rows(i)
                    If _Data.Any(Function(q) q.Col = r.Item(0).ToString()) Then Continue For
                    Dim nr As DataRow = dt.NewRow
                    nr.ItemArray = _Columns.Rows(i).ItemArray
                    dt.Rows.Add(nr)
                Next
                dt.AcceptChanges()
                red.DataSource = dt
                e.RepositoryItem = red
            End If
        End Sub

        Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
            If _Data.Any(Function(q) String.IsNullOrEmpty(q.Col)) Then Return
            If _Data.Count = _Columns.Rows.Count Then Return
            Me.GridView.AddNewRow()
        End Sub

        Private Sub GridView_InitNewRow(sender As Object, e As InitNewRowEventArgs) Handles GridView.InitNewRow
            'Me.GridView.SetRowCellValue(e.RowHandle, "SortOn", DevExpress.XtraGrid.ColumnSortMode.Default)
            'Me.GridView.SetRowCellValue(e.RowHandle, "Direction", DevExpress.Data.ColumnSortOrder.Descending)
            Me.GridView.SetRowCellValue(e.RowHandle, "Order", Me.GridView.RowCount - 1)
        End Sub


        Public Class SortElement
            Public Property Col As String
            Public Property SortOn As DevExpress.XtraGrid.ColumnSortMode = DevExpress.XtraGrid.ColumnSortMode.Default
            Public Property Direction As DevExpress.Data.ColumnSortOrder = DevExpress.Data.ColumnSortOrder.Descending
            Public Property Order As Integer

        End Class

        Private Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
            Me.SaveChanges()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK

        End Sub

        Public Sub SaveChanges()
            _View.BeginSort()
            _View.ClearSorting()
            Dim x As Integer = 0
            For Each elm As SortElement In _Data.OrderBy(Function(q) q.Order)
                If String.IsNullOrEmpty(elm.Col) Then Continue For
                Dim column As GridColumn = Me.View.Columns(elm.Col)
                If column Is Nothing Then Continue For
                column.SortMode = elm.SortOn
                column.SortOrder = elm.Direction
                column.SortIndex = x
                x += 1
            Next
            _View.EndSort()
        End Sub

        Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        End Sub

        Private Sub btnRemove_Click(sender As Object, e As System.EventArgs) Handles btnRemove.Click
            If Me.GridView.GetFocusedRow Is Nothing Then Return
            Dim index As Integer = System.Convert.ToInt32(Me.GridView.GetFocusedRowCellValue("Order"))
            Me.GridView.BeginDataUpdate()
            Dim elm As SortElement = _Data.FirstOrDefault(Function(q) q.Order = index)
            _Data.Remove(elm)
            Dim lst As List(Of SortElement) = _Data.OrderBy(Function(q) q.Order).ToList
            Dim x As Integer = 0
            For Each se As SortElement In lst
                se.Order = x
                x += 1
            Next
            Me.GridView.EndDataUpdate()

        End Sub
    End Class
End Namespace
