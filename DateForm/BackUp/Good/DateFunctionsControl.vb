Imports AcurSoft.Data.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class DateFunctionsControl

    Public Event OnRangeCriteriaChanged(e As DateRangeCriteria)


    Public ReadOnly Property RangeCriteria As CriteriaOperator
    Public ReadOnly Property RangeCriteriaBetween As CriteriaOperator

    Private _DicRangeStart As Dictionary(Of Integer, String)
    Public ReadOnly Property DicRangeStart As Dictionary(Of Integer, String)
        Get
            If _DicRangeStart Is Nothing Then

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

            End If
            Return _DicRangeStart
        End Get
    End Property
    Public Property PropertyName As String = "Property"
    'Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges
    Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges

    Private _RangeCriteriaEditing As Boolean

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

    Public ReadOnly Property DateChooseHelper As DateChooseHelper
        Get
            If Me.LookUpEditRange.EditValue Is Nothing OrElse Me.LookUpEditMonthAfter.EditValue Is Nothing Then Return Nothing
            Dim rtn As New DateChooseHelper(Convert.ToInt32(Me.LookUpEditRange.EditValue))
            rtn.AfterAgoValue = Convert.ToInt32(Me.LookUpEditMonthAfter.EditValue)
            Return rtn
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

#Region "Init Data"

    Public Sub InitLookUpEditor(editor As LookUpEdit, dic As Dictionary(Of Integer, String), Optional rowsCount As Integer = 0)
        With editor.Properties
            .DataSource = dic
            .ShowHeader = False
            .ShowFooter = False
            If rowsCount = -1 Then
                .DropDownRows = dic.Count
            ElseIf rowsCount <> 0
                .DropDownRows = rowsCount
            End If
        End With
    End Sub

    Public Sub InitEditors()
        Me.InitLookUpEditor(Me.LookUpEditRange, Me.DicRangeStart)

        AddHandler Me.CheckEditYear.CheckedChanged,
            Sub(s, a)
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            End Sub

        Dim dicQuaterMonthChoice As New Dictionary(Of Integer, String)
        With dicQuaterMonthChoice
            .Add(DateStartKind.Unkown, "")
            .Add(DateStartKind.Month, "Month")
            .Add(DateStartKind.Quarter, "Quarter")
        End With
        Me.InitLookUpEditor(Me.LookUpEditQuaterMonth, dicQuaterMonthChoice, 3)


        Dim dicThisAfterAgo As New Dictionary(Of Integer, String)
        With dicThisAfterAgo
            .Add(0, "This")
            .Add(1000, "After")
            .Add(-1000, "Ago")
        End With

        Dim dicQuater As Dictionary(Of Integer, String) = dicThisAfterAgo.ToDictionary(Function(q) q.Key, Function(q) q.Value)
        For i As Integer = 1 To 4
            dicQuater.Add(i, "Q" & i)
        Next
        Me.InitLookUpEditor(Me.LookUpEditQuater, dicQuater, -1)



        Dim dicMonth As Dictionary(Of Integer, String) = dicThisAfterAgo.ToDictionary(Function(q) q.Key, Function(q) q.Value)
        Dim dateFoo As New Date(2016, 1, 1)
        For i As Integer = 1 To 12
            dicMonth.Add(i, String.Format("{0:d2}: {1}", i, Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(dateFoo.AddMonths(i - 1).ToString("MMMM"))))
        Next
        Me.InitLookUpEditor(Me.LookUpEditMonth, dicMonth, -1)

        Dim dicFirstDayOfWeek As New Dictionary(Of Integer, String)
        For i As Integer = 0 To 6
            dicFirstDayOfWeek.Add(i, DirectCast(i, DayOfWeek).ToString)
        Next
        Me.InitLookUpEditor(Me.LookUpEditFirstDayOfWeek, dicFirstDayOfWeek, -1)

        Dim dicMonthWeek As New Dictionary(Of Integer, String)
        'With dicMonthWeek
        '    .Add(1, "1st Week")
        '    .Add(2, "2nd Week")
        '    .Add(3, "3rd Week")
        '    .Add(1001, "1st Full Week")
        '    .Add(1002, "2nd Full Week")
        '    .Add(1003, "3rd Full Week")
        '    .Add(-1, "Last Week")
        '    .Add(-1001, "Last Full Week")
        'End With
        'Dim x = 1 << 5
        'Dim y = x.GetExponents()
        With dicMonthWeek
            .Add(MonthWeekKind.FirstWeek, "1st Week")
            .Add(MonthWeekKind.SecondWeek, "2nd Week")
            .Add(MonthWeekKind.ThirdWeek, "3rd Week")
            .Add(MonthWeekKind.FirstFullWeek, "1st Full Week")
            .Add(MonthWeekKind.SecondFullWeek, "2nd Full Week")
            .Add(MonthWeekKind.ThirdFullWeek, "3rd Full Week")
            .Add(MonthWeekKind.LastWeek, "Last Week")
            .Add(MonthWeekKind.LastFullWeek, "Last Full Week")
        End With

        Me.InitLookUpEditor(Me.LookUpEditMonthWeek, dicMonthWeek, -1)


        _RangeCriteriaEditing = True
        Me.SpinEditYear.EditValue = Date.Today.Year
        Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.This).Value
        Me.LookUpEditQuaterMonth.EditValue = -1
        Me.LookUpEditQuater.EditValue = 0
        Me.LookUpEditMonth.EditValue = 0
        Me.LookUpEditFirstDayOfWeek.EditValue = 1
        Me.LookUpEditMonthWeek.EditValue = Convert.ToInt32(MonthWeekKind.FirstWeek)
        _RangeCriteriaEditing = False

    End Sub

#End Region

    Public Sub New()
        InitializeComponent()
        If Me.DesignMode Then Return

        Me.InitEditors()
        Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.RangeFunctionsEx
        If Me.RangeFunctionsEx.Count > 0 Then
            Me.GridLookUpEditRangeCriteria.EditValue = Me.RangeFunctionsEx.FirstOrDefault.DateFunction
        End If
    End Sub

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
        'Dim value As DateChooseHelper = DateChooseHelper.Instance(Convert.ToInt32(_RangeFunction.Kind))
        Dim dateStartKind As DateStartKind = _RangeFunction.DateStartKind
        If _RangeFunction.NeedMonthWeekNumber Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedMonth)
            'Me.LookUpEditMonthWeek.EditValue = 1
            Me.LookUpEditMonthWeek.EditValue = Convert.ToInt32(MonthWeekKind.FirstWeek)
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        ElseIf dateStartKind = DateStartKind.Month Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedMonth)
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf dateStartKind = DateStartKind.Quarter Then
            Me.LookUpEditQuaterMonth.EditValue = Convert.ToInt32(DateFunctionKind.NeedQuarter)
            Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciMonthWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciFirstDayOfWeek.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf dateStartKind = DateStartKind.Year Then
            Me.LookUpEditQuaterMonth.EditValue = 0
            Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End If
        Dim oldEditRange As Nullable(Of Integer) = Nothing
        If Me.LookUpEditRange.EditValue IsNot Nothing Then
            oldEditRange = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        End If
        Dim dicEditRange As Dictionary(Of Integer, String) = Nothing
        'Dim h As New DateChooseHelper(_RangeFunction.ChooseKind)
        Select Case dateStartKind
            Case DateStartKind.Year, DateStartKind.Month, DateStartKind.Quarter
                dicEditRange = Me.DicRangeStart.Where(Function(q) New DateChooseHelper(q.Key).DateStartKind = dateStartKind).ToDictionary(Function(q) q.Key, Function(q) q.Value)
        End Select
        If dicEditRange Is Nothing Then
            dicEditRange = Me.DicRangeStart
        End If
        Me.LookUpEditRange.Properties.DataSource = dicEditRange
        If Not oldEditRange.HasValue OrElse Not dicEditRange.ContainsKey(oldEditRange.Value) Then
            Select Case dateStartKind
                Case DateStartKind.Month, DateStartKind.Quarter
                    Me.UpdateAfterAgo(dateStartKind)
                Case Else
                    Me.LookUpEditRange.EditValue = dicEditRange.First.Key
            End Select
        End If
        If _RangeFunction IsNot Nothing Then
            Me.UpdateRangeCriteria()
        End If
        _RangeCriteriaEditing = False
    End Sub

    Public Sub UpdateRangeCriteria()
        Dim dateChooseHelper As DateChooseHelper = Me.DateChooseHelper
        If dateChooseHelper Is Nothing Then Return

        'Dim config As Integer = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        Dim year As Integer = 0
        If Not Me.CheckEditYear.Checked Then
            year = Convert.ToInt32(Me.SpinEditYear.Value)
        End If
        Dim month As Integer = Convert.ToInt32(Me.LookUpEditMonth.EditValue)

        Dim quarter As Integer = Convert.ToInt32(Me.LookUpEditQuater.EditValue)

        Dim monthWeekNumber As Integer = Convert.ToInt32(Me.LookUpEditMonthWeek.EditValue)
        Dim fullWeek As Boolean = Math.Abs(monthWeekNumber) > MonthWeekKind.FullWeek
        monthWeekNumber = monthWeekNumber Mod MonthWeekKind.FullWeek

        Dim firstDayOfWeek As Integer = Convert.ToInt32(Me.LookUpEditFirstDayOfWeek.EditValue)

        Dim dateRangeCriteria As New DateRangeCriteria(dateChooseHelper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)

        RaiseEvent OnRangeCriteriaChanged(dateRangeCriteria.UpdateRangeCriteria(_RangeFunction, Me.PropertyName))
        'Me.LabelCriteria.Text = dateRangeCriteria.RangeCriteria.ToString
        'Me.LabelCriteriaBetween.Text = dateRangeCriteria.RangeCriteriaBetweenText

    End Sub

    Private Sub SpinEditYear_EditValueChanged(sender As Object, e As EventArgs) Handles SpinEditYear.EditValueChanged, LookUpEditQuater.EditValueChanged, LookUpEditMonthWeek.EditValueChanged, LookUpEditMonth.EditValueChanged, LookUpEditFirstDayOfWeek.EditValueChanged, LookUpEditMonthAfter.EditValueChanged, CheckEditYear.CheckedChanged
        If _RangeCriteriaEditing Then Return
        _RangeCriteriaEditing = True
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
        Dim dateChooseHelper As DateChooseHelper = Me.DateChooseHelper
        If dateChooseHelper Is Nothing Then Return

        'Dim dateChooseHelper As DateChooseHelper = DateChooseHelper.Instance(Convert.ToInt32(Me.LookUpEditRange.EditValue))
        If dateChooseHelper.DateStartKind = DateStartKind.Year Then
            Me.UpdateRangeCriteria()
        End If

        If dateChooseHelper.DateChooseKind = DateChooseKind.This Then
            If dateChooseHelper.DateStartKind = DateStartKind.Year Then
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
                If dateChooseHelper.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                ElseIf dateChooseHelper.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                End If

            End If
            Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        ElseIf dateChooseHelper.DateChooseKind = DateChooseKind.After OrElse dateChooseHelper.DateChooseKind = DateChooseKind.Ago
            If dateChooseHelper.DateStartKind = DateStartKind.Year Then
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
                If dateChooseHelper.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                ElseIf dateChooseHelper.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            End If
            Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
        ElseIf dateChooseHelper.DateChooseKind = DateChooseKind.Choose
            If dateChooseHelper.DateStartKind = DateStartKind.Month OrElse dateChooseHelper.DateStartKind = DateStartKind.Quarter Then
                Me.lcgRangConfig.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
                If dateChooseHelper.DateStartKind = DateStartKind.Month Then
                    Me.lciMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                ElseIf dateChooseHelper.DateStartKind = DateStartKind.Quarter Then
                    Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            ElseIf dateChooseHelper.DateStartKind = DateStartKind.Year
                Me.lcgRangeYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lciThisYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciMonthAfter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            End If
        End If
    End Sub

    Private Sub LookUpEditQuaterMonth_EditValueChanged(sender As Object, e As EventArgs) Handles LookUpEditQuaterMonth.EditValueChanged
        Dim dateStartKind As DateStartKind = DirectCast(Convert.ToInt32(Me.LookUpEditQuaterMonth.EditValue), DateStartKind)
        Select Case dateStartKind
            Case DateStartKind.Unkown
                Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.RangeFunctionsEx
                Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Case DateStartKind.Month
                Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Case DateStartKind.Quarter
                Me.lciQuater.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                Me.lcgMonth.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
        End Select
    End Sub
End Class
