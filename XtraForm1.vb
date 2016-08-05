Imports System.IO
Imports System.Reflection
Imports AcurSoft.XtraGrid.Columns.Helpers
Imports AcurSoft.XtraGrid.Views.Grid
Imports DevExpress.Data.Filtering
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors.ViewInfo
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Menu

Public Class XtraForm1

    Public Sub New()
        AcurSoft.Data.Filtering.XpoFunctionsHelper.Register()
        XpoFunctionFilterManager.Init()


        CriteriaOperator.RegisterCustomFunction(New RemoveDiacriticsFunction())
        InitializeComponent()
        GridControlEx1.DataSource = GetData(50)
        'GridViewEx1.ActiveFilterString = "[ID] Between (3, 6)"
        'Dim pp = Me.GridViewEx1.ActiveFilterCriteria
        AddHandler GridViewEx1.CustomFilterPanelButtonClick, AddressOf customGridView1_CustomButtonClick
        AddCustomButtonsToFilterPanel()
        'GridViewEx1.OptionsView.FillEmptySpace = True

        GridViewEx1.OptionsFind.AlwaysVisible = True
        GridViewEx1.Columns("Date").OptionsFilter.FilterPopupMode = FilterPopupModeExtended.Date
        GridViewEx1.Columns("Date").OptionsFilter.UseFilterPopupRangeDateMode = True
        GridViewEx1.OptionsBehavior.ColumnsSelectionHelper.DrawCheckBoxMode = DrawCheckBoxModeEnum.IsCheckedAndOnHover
        GridViewEx1.OptionsBehavior.BookMarksKeyFieldName = "ID"
        GridViewEx1.OptionsBehavior.Editable = False
        GridViewEx1.OptionsMenu.ShowConditionalFormattingItem = True

        'PopupMenu1.AddItem(New DevExpress.XtraBars.BarListItem() With {.Caption = "test"})

        'rangeControlRange.Minimum =
        Dim dateTimeChartRangeControlClient As New DateTimeChartRangeControlClient
        Me.RangeControl1.Client = dateTimeChartRangeControlClient
        dateTimeChartRangeControlClient.DataProvider.DataSource = dt
        'dateTimeChartRangeControlClient.DataProvider.ValueDataMember = "Value"
        dateTimeChartRangeControlClient.DataProvider.ValueDataMember = "Value"
        'dateTimeChartRangeControlClient.DataProvider.SeriesDataMember = "Date"
        dateTimeChartRangeControlClient.DataProvider.ArgumentDataMember = "Date"

        AddHandler dateTimeChartRangeControlClient.CustomizeSeries,
            Sub(s, a)
                'a.DataSourceValue
            End Sub

        Dim rangeControlRange As New RangeControlRange() With {.Owner = Me.RangeControl1}
        Me.RangeControl1.SelectedRange = rangeControlRange
        rangeControlRange.Minimum = dt.Rows.OfType(Of DataRow).Min(Function(q) q.Field(Of Date)("Date"))
        rangeControlRange.Maximum = dt.Rows.OfType(Of DataRow).Max(Function(q) q.Field(Of Date)("Date"))

        AddHandler RangeControl1.RangeChanged, AddressOf RangeControl1_RangeChanged
        Dim o = 0
    End Sub

    Private Sub AddCustomButtonsToFilterPanel()
        Dim b As New EditorButton(ButtonPredefines.Glyph) With {.Caption = "Custom Button", .Width = 80}
        GridViewEx1.AddFilterPanelButton(b)

        b = New EditorButton(ButtonPredefines.Glyph) With {.Caption = "Custom Filter Button", .Width = 125}
        b.Image = Image.FromFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName & "\Filter_16x16.png")
        b.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft
        GridViewEx1.AddFilterPanelButton(b)
    End Sub

    Private Sub customGridView1_CustomButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim b As EditorButton = TryCast(sender, EditorButton)
        Console.WriteLine(String.Format("{0} is clicked", b.Caption))
        Dim x = GridViewEx1.GetCheckedColumns
        Dim rr = 0
    End Sub

    Private dt As DataTable
    Private Function GetData(ByVal rows As Integer) As DataTable
        dt = New DataTable()
        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("Info", GetType(String))
        dt.Columns.Add("Value", GetType(Decimal))
        dt.Columns.Add("Date", GetType(Date))
        dt.Columns.Add("Type", GetType(String))
        dt.Columns.Add("g", GetType(Guid))
        dt.Columns.Add("ts", GetType(TimeSpan))
        For i As Integer = 0 To rows - 1
            dt.Rows.Add(i, "Infà" & i, 3.37 * i, Date.Now.AddDays(i), "Typé " & i Mod 3, Guid.NewGuid, New TimeSpan(0, 5, 1, 36 + i, 0))
        Next i
        Return dt
    End Function

    Private Sub GridViewEx1_ColumnPositionChanged(sender As Object, e As EventArgs) Handles GridViewEx1.ColumnPositionChanged
        Dim x = 0
    End Sub

    Private Sub GridViewEx1_PopupMenuShowing(sender As Object, e As DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs) Handles GridViewEx1.PopupMenuShowing
        If e.HitInfo.HitTest = DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowIndicator Then Return

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs)
        Me.GridViewEx1.OptionsView.ColumnAutoWidth = False
    End Sub

    Private Sub ButtonEdit1_ButtonClick(sender As Object, e As ButtonPressedEventArgs)
        Dim editor As ButtonEdit = DirectCast(sender, ButtonEdit)

        Dim bvi As EditorButtonObjectInfoArgs = DirectCast(editor.GetViewInfo(), ButtonEditViewInfo).ButtonInfoByButton(e.Button)

        Dim pt As New Point(bvi.Bounds.Left, bvi.Bounds.Bottom)

        'pt = editor.PointToScreen(pt)
        Dim menu As New DevExpress.Utils.Menu.DXPopupStandardMenu(New DXPopupMenu)  '(Me.GridViewEx1)
        menu.Menu.Items.Add(New DevExpress.Utils.Menu.DXMenuItem("text"))
        menu.Show(editor, pt)
        'PopupMenu1.ShowPopup(pt)
    End Sub

    Private Sub RangeControl1_RangeChanged(sender As Object, range As RangeControlRangeEventArgs) 'Handles RangeControl1.RangeChanged
        'Dim x = range.Range
        'Dim cr As CriteriaOperator = Me.GridViewEx1.Columns.Item("Date").FilterInfo.FilterCriteria
        'cr = cr And New BetweenOperator("Date", New OperandValue(range.Range.Minimum), New OperandValue(range.Range.Maximum))
        'Me.GridViewEx1.ActiveFilterCriteria = cr
        Dim cfi As New ColumnFilterInfo(New BetweenOperator("Date", New OperandValue(range.Range.Minimum), New OperandValue(range.Range.Maximum)))
        Me.GridViewEx1.Columns.Item("Date").FilterInfo = cfi
        Dim z = 0
    End Sub

    Private Sub SimpleButton1_Click_1(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        'Me.PopupContainerControl1.PopupContainerProperties.
        Dim fd As FilterPopupDateEditForm = FilterPopupDateEditForm.CreateAndShow(EditModeEnum.Normal, New Point(100, 100))
        fd.IsRangeCheckEdit.Visible = False
        'fd.SetCriteria(CriteriaOperator.Parse("[test] > #2016-05-05#"))
        AddHandler fd.OnResult,
            Sub(b)
                If Not b Then
                    Dim p = fd.GetCriteria("test")
                    fd.SetCriteria(p)
                End If
            End Sub


        'fd.Location = New Point(100, 100)
        'fd.ShowDialog()
        'fd.Show()
        'Dim fd As New PopupDateEditForm(New DateEdit)
        'fd.Location = New Point(100, 100)
        'Dim cld As CalendarControl = TryCast(fd.Calendar, CalendarControl)
        'cld.SelectionMode = Repository.CalendarSelectionMode.Single
        'cld.EditValue = Date.Today
        'fd.Size = New Size(cld.Width * 2, cld.Height + 1)
        'cld.Dock = DockStyle.Left
        'Dim cld2 As New CalendarControl
        'cld2.SelectionMode = Repository.CalendarSelectionMode.Single
        'cld2.EditValue = Date.Today.AddDays(5)
        'fd.Controls.Add(cld2)
        'cld2.Dock = DockStyle.Left
        'cld.SendToBack()



        'fd.Show()

        'Dim f As New RemoveDiacriticsFunction()
        'Dim s = f.Evaluate("testééà")
        'Dim cr = GridFilterAccentSubstitutor.Substitute(GridFilterAccentSubstitutor.WrapIntoCustomFunction(CriteriaOperator.Parse("'testééà'")))

        Dim r = 0
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Dim editor As New DateCriteriaEdit With {.Width = 500}
        editor.Properties.EditMode = EditModeEnum.Between
        Me.Controls.Add(editor)
        editor.BringToFront()

    End Sub

    Private Sub SimpleButton3_Click(sender As Object, e As EventArgs) Handles SimpleButton3.Click
        Me.GridViewEx1.SaveLayoutToXml("test.xml")
        'Me.GridView1.SaveLayoutToXml("testo.xml")
    End Sub
    Private Sub SimpleButton4_Click(sender As Object, e As EventArgs) Handles SimpleButton4.Click
        Me.GridViewEx1.RestoreLayoutFromXml("test.xml")
        'Me.GridView1.RestoreLayoutFromXml("testo.xml")

    End Sub


    Private filterRow As Boolean
    Private Sub SimpleButton5_Click(sender As Object, e As EventArgs) Handles SimpleButton5.Click
        filterRow = Not filterRow
        'Me.GridViewEx1.OptionsBehavior.BookmarksHelper. = filterRow
    End Sub
    'Private Sub DateRangeFunctionsControl1_OnRangeCriteriaChanged(e As AcurSoft.Data.Filtering.DateRangeCriteria)
    '    Dim x = e
    'End Sub



    'Private Sub GridViewEx1_SubstituteFilter(sender As Object, e As DevExpress.Data.SubstituteFilterEventArgs) Handles GridViewEx1.SubstituteFilter
    '    Dim x = e.Filter
    '    Dim z = 0
    '    e.Filter = GridFilterAccentSubstitutor.Substitute(e.Filter)

    'End Sub
End Class