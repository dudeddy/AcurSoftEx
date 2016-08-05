Imports AcurSoft.Data.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class DateFunctionsControl


    Public Property InfoStart As DateCriteriaFunctionInfo
    Public Property InfoEnd As DateCriteriaFunctionInfo
    Public Property InfoCriteria As DateCriteriaFunctionInfo
    Public Property InfoRange As DateCriteriaFunctionInfo
    Private _DicRangeStart As Dictionary(Of Integer, String)


    Private _Functions As List(Of DateCriteriaFunctionInfo)
    Public ReadOnly Property Functions As List(Of DateCriteriaFunctionInfo)
        Get
            If _Functions Is Nothing Then
                _Functions = CriteriaOperator.GetCustomFunctions.OfType(Of XpoFunctionForDates).Where(Function(q) q.Kind = DateFunctionKind.GetDate).Select(
                    Function(q) q.GetInfo()).ToList()

            End If
            Return _Functions
        End Get
    End Property

    Private _RangeFunctions As List(Of DateCriteriaFunctionInfo)
    Public ReadOnly Property RangeFunctions As List(Of DateCriteriaFunctionInfo)
        Get
            If _RangeFunctions Is Nothing Then
                _RangeFunctions = CriteriaOperator.GetCustomFunctions.OfType(Of XpoFunctionForDates).Where(Function(q) q.Kind = DateFunctionKind.GetRange).Select(
                    Function(q) q.GetInfo()).ToList()

            End If
            Return _RangeFunctions
        End Get
    End Property

    Public ReadOnly Property InYearFunctionInfo As DateCriteriaFunctionInfo = RangeFunctionsEx.FirstOrDefault(Function(q) q.Caption = "InYear")

    Private _RangeFunctionsEx As List(Of DateCriteriaFunctionInfo)
    Public ReadOnly Property RangeFunctionsEx As List(Of DateCriteriaFunctionInfo)
        Get
            If _RangeFunctionsEx Is Nothing Then
                _RangeFunctionsEx = CriteriaOperator.GetCustomFunctions.OfType(Of XpoFunctionForDateRanges).Select(
                    Function(q) q.GetInfo()).ToList()

            End If
            Return _RangeFunctionsEx
        End Get
    End Property


    Public Sub UpdateAfterAgo(dateStartKind As DateStartKind)
        Dim editor As LookUpEdit = Nothing
        Select Case dateStartKind
            Case DateStartKind.Month
                editor = Me.LookUpEditMonth
            Case DateStartKind.Quarter
                editor = Me.LookUpEditQuater
        End Select

        If editor Is Nothing OrElse editor.EditValue Is Nothing Then Return
        Dim value As Integer = Convert.ToInt32(editor.EditValue)
        Select Case value
            Case 0
                Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(dateStartKind, DateChooseKind.This).Value
            Case -1000
                Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(dateStartKind, DateChooseKind.Ago).Value
            Case 1000
                Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(dateStartKind, DateChooseKind.After).Value
            Case Else
                Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(dateStartKind, DateChooseKind.Choose).Value
        End Select

    End Sub

    Public Sub New()
        InitializeComponent()
        If Me.DesignMode Then Return

        _DicRangeStart = New Dictionary(Of Integer, String)
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.This).Value, "This Year")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.Choose).Value, "Year")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.After).Value, "After N Year(s)")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.Ago).Value, "N Year(s) Ago")

        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.This).Value, "This Month")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.Choose).Value, "Month")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.After).Value, "After N Month(s)")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.Ago).Value, "N Month(s) Ago")

        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.This).Value, "This Quarter")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.Choose).Value, "Quarter")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.After).Value, "After N Quarter(s)")
        _DicRangeStart.Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.Ago).Value, "N Quarter(s) Ago")

        With Me.LookUpEditRange.Properties
            .DataSource = _DicRangeStart
            .ShowHeader = False
            .ShowFooter = False
        End With
        Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.This).Value



        Me.SpinEditYear.EditValue = Date.Today.Year
        AddHandler Me.CheckEditYear.CheckedChanged,
            Sub(s, a)
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            End Sub
        Dim dicQuaterMonthChoice As New Dictionary(Of Integer, String)
        dicQuaterMonthChoice.Add(-1, "")
        dicQuaterMonthChoice.Add(DateFunctionKind.NeedMonth, "Month")
        dicQuaterMonthChoice.Add(DateFunctionKind.NeedQuarter, "Quarter")
        With Me.LookUpEditQuaterMonth.Properties
            .DataSource = dicQuaterMonthChoice
            .ShowHeader = False
            .ShowFooter = False
            .DropDownRows = 3
        End With
        Me.LookUpEditQuaterMonth.EditValue = -1


        Dim dicQuater As New Dictionary(Of Integer, String)
        dicQuater.Add(0, "This")
        dicQuater.Add(1000, "After")
        dicQuater.Add(-1000, "Ago")
        dicQuater.Add(1, "Q1")
        dicQuater.Add(2, "Q2")
        dicQuater.Add(3, "Q3")
        dicQuater.Add(4, "Q4")
        With Me.LookUpEditQuater.Properties
            .DataSource = dicQuater
            .ShowHeader = False
            .ShowFooter = False
            .DropDownRows = 5
        End With
        Me.LookUpEditQuater.EditValue = 0

        Dim dicMonth As New Dictionary(Of Integer, String)
        dicMonth.Add(0, "This")
        dicMonth.Add(1000, "After")
        dicMonth.Add(-1000, "Ago")
        Dim dateFoo As New Date(2016, 1, 1)
        For i As Integer = 1 To 12
            dicMonth.Add(i, String.Format("[{0}] {1:MMMM}", i, dateFoo.AddMonths(i - 1)))
        Next
        With Me.LookUpEditMonth.Properties
            .DataSource = dicMonth
            .ShowHeader = False
            .ShowFooter = False
        End With
        Me.LookUpEditMonth.EditValue = 0

        Dim dicFirstDayOfWeek As New Dictionary(Of Integer, String)
        For i As Integer = 0 To 6
            dicFirstDayOfWeek.Add(i, DirectCast(i, DayOfWeek).ToString)
        Next
        With Me.LookUpEditFirstDayOfWeek.Properties
            .DataSource = dicFirstDayOfWeek
            .ShowHeader = False
            .ShowFooter = False
            .DropDownRows = 7
        End With
        Me.LookUpEditFirstDayOfWeek.EditValue = 1



        Dim dicMonthWeek As New Dictionary(Of Integer, String)
        'dicMonthWeek.Add(0, "")
        dicMonthWeek.Add(1, "1st Week")
        dicMonthWeek.Add(2, "2nd Week")
        dicMonthWeek.Add(3, "3rd Week")
        dicMonthWeek.Add(1001, "1st Full Week")
        dicMonthWeek.Add(1002, "2nd Full Week")
        dicMonthWeek.Add(1003, "3rd Full Week")
        dicMonthWeek.Add(-1, "Last Week")
        dicMonthWeek.Add(-1001, "Last Full Week")
        With Me.LookUpEditMonthWeek.Properties
            .DataSource = dicMonthWeek
            .ShowHeader = False
            .ShowFooter = False
            .DropDownRows = dicMonthWeek.Count
        End With
        Me.LookUpEditMonthWeek.EditValue = 1

        Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.RangeFunctionsEx
        If Me.RangeFunctionsEx.Count > 0 Then
            Me.GridLookUpEditRangeCriteria.EditValue = Me.RangeFunctionsEx.FirstOrDefault.DateFunction
        End If

        Me.GridControlStart.DataSource = Me.Functions
        Me.GridControlEnd.DataSource = Me.Functions
        Me.GridControlCriteria.DataSource = Me.Functions
        Me.GridControlCriteria.DataSource = Me.RangeFunctions
        'AddHandler Me.GridViewCriteria.FocusedRowChanged,
        '    Sub(s, a)
        '        Me.InfoCriteria = Me.FocusedRowChanged(s, a)
        '    End Sub
        'AddHandler Me.GridViewStart.FocusedRowChanged,
        '    Sub(s, a)
        '        Me.InfoStart = Me.FocusedRowChanged(s, a)
        '    End Sub
        'AddHandler Me.GridViewEnd.FocusedRowChanged,
        '    Sub(s, a)
        '        Me.InfoEnd = Me.FocusedRowChanged(s, a)
        '    End Sub
        'AddHandler Me.GridViewRanges.FocusedRowChanged,
        '    Sub(s, a)
        '        Me.InfoCriteria = Me.FocusedRowChanged(s, a)
        '    End Sub

    End Sub

    Private Sub CheckEditDateStart_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEditDateStart.CheckedChanged
        If Me.DesignMode Then Return
        Me.GridControlStart.Enabled = Me.CheckEditCriteriaStart.Checked
    End Sub

    Private Sub CheckEditDateEnd_CheckedChanged(sender As Object, e As EventArgs) Handles CheckEditDateEnd.CheckedChanged
        If Me.DesignMode Then Return
        Me.GridControlEnd.Enabled = Me.CheckEditCriteriaEnd.Checked
    End Sub


    Public Function FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) As DateCriteriaFunctionInfo
        If Me.DesignMode Then Return Nothing
        Dim gridView As GridView = DirectCast(sender, GridView)
        Dim info As DateCriteriaFunctionInfo = DirectCast(gridView.GetRow(e.FocusedRowHandle), DateCriteriaFunctionInfo)
        Return info
    End Function

    'Public ReadOnly Property RangeCriteria As CriteriaOperator
    'Public ReadOnly Property RangeCriteriaBetween As CriteriaOperator

    Public Property CurrentDate As Date = Date.Today
    Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges

    Private _RangeCriteriaEditing As Boolean
    Private Sub GridLookUpEditRangeCriteria_EditValueChanged(sender As Object, e As EventArgs) Handles GridLookUpEditRangeCriteria.EditValueChanged
        If Me.DesignMode Then Return
        If _RangeCriteriaEditing Then Return
        If Me.GridLookUpEditRangeCriteria.EditValue Is Nothing Then
            Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Return
        End If
        Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

        _RangeCriteriaEditing = True
        _RangeFunction = DirectCast(Me.GridLookUpEditRangeCriteria.EditValue, XpoFunctionForDateRanges)
        If _RangeFunction.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedMonth)
            Me.LookUpEditMonthWeek.EditValue = 1
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        ElseIf _RangeFunction.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedMonth)
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf _RangeFunction.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedQuarter)
            Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf _RangeFunction.Kind.HasFlag(DateFunctionKind.NeedYear) Then
            Me.LookUpEditQuaterMonth.EditValue = 0
            Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        Dim oldEditRange As Nullable(Of Integer) = Nothing
        If Me.LookUpEditRange.EditValue IsNot Nothing Then
            oldEditRange = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        End If
        Dim dicEditRange As Dictionary(Of Integer, String) = Nothing
        Dim h As New DateChooseHelper(_RangeFunction.ChooseKind)
        Select Case h.DateStartKind
            Case DateStartKind.Year, DateStartKind.Month, DateStartKind.Quarter
                dicEditRange = _DicRangeStart.Where(Function(q) New DateChooseHelper(q.Key).DateStartKind = h.DateStartKind).ToDictionary(Function(q) q.Key, Function(q) q.Value)
        End Select
        If dicEditRange Is Nothing Then
            dicEditRange = _DicRangeStart
        End If
        Me.LookUpEditRange.Properties.DataSource = dicEditRange
        If Not oldEditRange.HasValue OrElse Not dicEditRange.ContainsKey(oldEditRange.Value) Then
            Select Case h.DateStartKind
                Case DateStartKind.Month, DateStartKind.Quarter
                    Me.UpdateAfterAgo(h.DateStartKind)
                Case Else
                    Me.LookUpEditRange.EditValue = dicEditRange.First.Key
            End Select
        End If
        If _RangeFunction IsNot Nothing Then
            Me.UpdateRangeCriteria()
        End If
        _RangeCriteriaEditing = False
    End Sub

    Public ReadOnly Property DateChooseHelper As DateChooseHelper
        Get
            If Me.LookUpEditRange.EditValue Is Nothing OrElse Me.LookUpEditMonthAfter.EditValue Is Nothing Then Return Nothing
            Dim rtn As New DateChooseHelper(Convert.ToInt32(Me.LookUpEditRange.EditValue))
            rtn.AfterAgoValue = Convert.ToInt32(Me.LookUpEditMonthAfter.EditValue)
            Return rtn
        End Get
    End Property


    Public Sub UpdateRangeCriteria()
        Dim dateChooseHelper As DateChooseHelper = Me.DateChooseHelper
        If dateChooseHelper Is Nothing Then Return

        Dim config As Integer = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        Dim year As Integer = 0
        If Not Me.CheckEditYear.Checked Then
            year = Convert.ToInt32(Me.SpinEditYear.Value)
        End If
        Dim month As Integer = Convert.ToInt32(Me.LookUpEditMonth.EditValue)

        Dim quarter As Integer = Convert.ToInt32(Me.LookUpEditQuater.EditValue)
        Dim monthWeekNumber As Integer = Convert.ToInt32(Me.LookUpEditMonthWeek.EditValue)
        Dim fullWeek As Boolean = Math.Abs(monthWeekNumber) > 1000
        monthWeekNumber = monthWeekNumber Mod 1000

        Dim firstDayOfWeek As Integer = Convert.ToInt32(Me.LookUpEditFirstDayOfWeek.EditValue)
        Dim dateRangeCriteria As New DateRangeCriteria(dateChooseHelper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)

        dateRangeCriteria.UpdateRangeCriteria(_RangeFunction, "test")
        Me.LabelCriteria.Text = dateRangeCriteria.RangeCriteria.ToString
        Me.LabelCriteriaBetween.Text = dateRangeCriteria.RangeCriteriaBetweenText

    End Sub

    Private Sub SpinEditYear_EditValueChanged(sender As Object, e As EventArgs) Handles SpinEditYear.EditValueChanged, LookUpEditQuater.EditValueChanged, LookUpEditMonthWeek.EditValueChanged, LookUpEditMonth.EditValueChanged, LookUpEditFirstDayOfWeek.EditValueChanged, LookUpEditMonthAfter.EditValueChanged, CheckEditYear.CheckedChanged
        If _RangeCriteriaEditing Then Return
        _RangeCriteriaEditing = True
        'If _RangeFunction Is Nothing Then
        '    _RangeFunction = DirectCast(InYearFunctionInfo.DateFunction, XpoFunctionForDateRanges)
        'End If
        If sender Is Me.LookUpEditMonth Then
            Me.UpdateAfterAgo(DateStartKind.Month)
        ElseIf sender Is Me.LookUpEditQuater Then
            Me.UpdateAfterAgo(DateStartKind.Quarter)
        End If
        If Me.LookUpEditRange.EditValue IsNot Nothing AndAlso _RangeFunction IsNot Nothing Then
            Me.UpdateRangeCriteria()
        End If

        _RangeCriteriaEditing = False

    End Sub

    Private Sub LookUpEditRange_EditValueChanged(sender As Object, e As EventArgs) Handles LookUpEditRange.EditValueChanged
        If Me.LookUpEditRange.EditValue Is Nothing OrElse _RangeFunction Is Nothing Then Return
        'Dim value As Integer = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        Dim value As DateChooseHelper = DateChooseHelper.Instance(Convert.ToInt32(Me.LookUpEditRange.EditValue))
        If value.DateStartKind = DateStartKind.Year Then
            Me.UpdateRangeCriteria()
        End If

        If value.DateChooseKind = DateChooseKind.This Then
            If value.DateStartKind = DateStartKind.Year Then
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
                If value.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                ElseIf value.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                End If

            End If
            Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf value.DateChooseKind = DateChooseKind.After OrElse value.DateChooseKind = DateChooseKind.Ago
            If value.DateStartKind = DateStartKind.Year Then
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Else
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
                If value.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                ElseIf value.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If


            End If
            Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

            '''If value.DateStartKind = DateStartKind.Month Then
            '''    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '''ElseIf value.DateStartKind = DateStartKind.Quarter Then
            '''    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '''    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            '''End If
        ElseIf value.DateChooseKind = DateChooseKind.Choose

            If value.DateStartKind = DateStartKind.Month OrElse value.DateStartKind = DateStartKind.Quarter Then
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
                If value.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                ElseIf value.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            ElseIf value.DateStartKind = DateStartKind.Year
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

                'ElseIf value.DateStartKind = DateStartKind.Quarter
                '    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                '    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                '    Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                '    Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                '    Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                '    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                '    Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End If
        End If
    End Sub

    'Private Sub DateFunctionsControl_Load(sender As Object, e As EventArgs) Handles Me.Load

    'End Sub

    Private Sub LookUpEditQuaterMonth_EditValueChanged(sender As Object, e As EventArgs) Handles LookUpEditQuaterMonth.EditValueChanged
        If Convert.ToInt32(Me.LookUpEditQuaterMonth.EditValue) = -1 Then
            Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.RangeFunctionsEx
            Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        Else
            Dim kind As DateFunctionKind = DirectCast(Me.LookUpEditQuaterMonth.EditValue, DateFunctionKind)
            Select Case kind
                Case DateFunctionKind.NeedMonth
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                    Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Case DateFunctionKind.NeedQuarter
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            End Select
        End If

    End Sub
End Class
