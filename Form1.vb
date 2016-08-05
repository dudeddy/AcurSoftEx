Imports DevExpress.XtraEditors.Controls
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Windows.Forms
Imports AcurSoft.XtraGrid.Columns.Helpers

Namespace dxExample
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
            customGrid1.DataSource = GetData(10)
            customGridView1.ActiveFilterString = "[ID] Between (3, 6)"
            AddHandler customGridView1.CustomFilterPanelButtonClick, AddressOf customGridView1_CustomButtonClick
            AddCustomButtonsToFilterPanel()

            'Dim dataTable As New DataTable()
            'dataTable.Columns.Add("Range Date", GetType(Date))
            'dataTable.Columns.Add("Event", GetType(String))
            'Dim row0() As Object = {Date.Today - New TimeSpan(1, 0, 0, 0), "Yesterday"}
            'Dim row1() As Object = {Date.Today, "Today"}
            'Dim row2() As Object = {Date.Today + New TimeSpan(1, 0, 0, 0), "Tomorrow"}
            'dataTable.Rows.Add(row0)
            'dataTable.Rows.Add(row1)
            'dataTable.Rows.Add(row2)
            'Me.customGrid1.DataSource = DataTable
            'Me.customGridView1.Columns("Date").ColumnEdit = riDateEdit
            customGridView1.Columns("Date").OptionsFilter.FilterPopupMode = FilterPopupModeExtended.Date
            customGridView1.Columns("Date").OptionsFilter.UseFilterPopupRangeDateMode = True
            customGridView1.OptionsBehavior.ColumnsSelectionHelper.DrawCheckBoxMode = DrawCheckBoxModeEnum.IsCheckedAndOnHover
        End Sub

        Private Sub AddCustomButtonsToFilterPanel()
            Dim b As New EditorButton(ButtonPredefines.Glyph) With {.Caption = "Custom Button", .Width = 80}
            customGridView1.AddFilterPanelButton(b)

            b = New EditorButton(ButtonPredefines.Glyph) With {.Caption = "Custom Filter Button", .Width = 125}
            b.Image = Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName & "\Filter_16x16.png")
            b.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
            customGridView1.AddFilterPanelButton(b)
        End Sub

        Private Sub customGridView1_CustomButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim b As EditorButton = TryCast(sender, EditorButton)
            Console.WriteLine(String.Format("{0} is clicked", b.Caption))
            Dim x = customGridView1.GetCheckedColumns
            Dim rr = 0
        End Sub

        Private Function GetData(ByVal rows As Integer) As DataTable
            Dim dt As New DataTable()
            dt.Columns.Add("ID", GetType(Integer))
            dt.Columns.Add("Info", GetType(String))
            dt.Columns.Add("Value", GetType(Decimal))
            dt.Columns.Add("Date", GetType(Date))
            dt.Columns.Add("Type", GetType(String))
            For i As Integer = 0 To rows - 1
                dt.Rows.Add(i, "Info" & i, 3.37 * i, Date.Now.AddDays(i), "Type " & i Mod 3)
            Next i
            Return dt
        End Function

    End Class
End Namespace
