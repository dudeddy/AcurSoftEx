Imports AcurSoft.Data.Filtering
Imports DevExpress.Data.Filtering
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class DateFunctionsControl

    Public Event OnCriteriaChanged(e As DateCriteria)


    'Public ReadOnly Property RangeCriteria As CriteriaOperator
    'Public ReadOnly Property RangeCriteriaBetween As CriteriaOperator
    Public Property PropertyName As String = "Property"
    'Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges
    Public ReadOnly Property DateFunction As XpoFunctionForDates

    Private _CriteriaEditing As Boolean

    Public Overridable Function GetDateCriteriaFunctionInfos() As List(Of DateCriteriaFunctionInfo)
        Return CriteriaOperator.GetCustomFunctions.OfType(Of XpoFunctionForDates).Where(Function(q) q.Kind.HasFlag(DateFunctionKind.IsDate)).Select(Function(q) q.GetInfo()).ToList()
    End Function

    Private _DateCriteriaFunctionInfos As List(Of DateCriteriaFunctionInfo)
    Public Overridable ReadOnly Property DateCriteriaFunctionInfos As List(Of DateCriteriaFunctionInfo)
        Get
            If _DateCriteriaFunctionInfos Is Nothing Then
                _DateCriteriaFunctionInfos = GetDateCriteriaFunctionInfos()
            End If
            Return _DateCriteriaFunctionInfos
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

    'Public ReadOnly Property EditorsDataSource As DateFunctionsControlData

    Public Sub InitEditors()
        Me.InitLookUpEditor(Me.LookUpEditRange, DateFunctionsControlData.FullThisAfterAgo)

        AddHandler Me.CheckEditYear.CheckedChanged,
            Sub(s, a)
                If Me.CheckEditYear.Checked Then
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                Else
                    Me.lciYear.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                End If
            End Sub
        Me.InitLookUpEditor(Me.LookUpEditQuaterMonth, DateFunctionsControlData.QuaterMonthChoiceData, 3)

        Me.InitLookUpEditor(Me.LookUpEditQuater, DateFunctionsControlData.Quarters, -1)

        Me.InitLookUpEditor(Me.LookUpEditMonth, DateFunctionsControlData.Months, -1)

        Me.InitLookUpEditor(Me.LookUpEditFirstDayOfWeek, DateFunctionsControlData.FirstDayOfWeeks, -1)

        Me.InitLookUpEditor(Me.LookUpEditMonthWeek, DateFunctionsControlData.MonthWeeks, -1)


        _CriteriaEditing = True
        Me.SpinEditYear.EditValue = Date.Today.Year
        Me.LookUpEditRange.EditValue = DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.This).Value
        Me.LookUpEditQuaterMonth.EditValue = -1
        Me.LookUpEditQuater.EditValue = 0
        Me.LookUpEditMonth.EditValue = 0
        Me.LookUpEditFirstDayOfWeek.EditValue = 1
        Me.LookUpEditMonthWeek.EditValue = Convert.ToInt32(MonthWeekKind.FirstWeek)
        _CriteriaEditing = False

    End Sub

#End Region

    Public Sub New()
        InitializeComponent()
        If Me.DesignMode Then Return

        Me.InitEditors()
        Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.DateCriteriaFunctionInfos
        If Me.DateCriteriaFunctionInfos.Count > 0 Then
            Me.GridLookUpEditRangeCriteria.EditValue = Me.DateCriteriaFunctionInfos.FirstOrDefault.DateFunction
        End If
    End Sub

    Private Sub GridLookUpEditRangeCriteria_EditValueChanged(sender As Object, e As EventArgs) Handles GridLookUpEditRangeCriteria.EditValueChanged
        If Me.DesignMode Then Return
        If _CriteriaEditing Then Return
        If Me.GridLookUpEditRangeCriteria.EditValue Is Nothing Then
            Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
            Return
        End If
        Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never

        _CriteriaEditing = True
        _DateFunction = DirectCast(Me.GridLookUpEditRangeCriteria.EditValue, XpoFunctionForDates)
        'Dim value As DateChooseHelper = DateChooseHelper.Instance(Convert.ToInt32(_RangeFunction.Kind))
        Dim dateStartKind As DateStartKind = _DateFunction.DateStartKind
        If _DateFunction.NeedMonthWeekNumber Then
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
            Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always

        End If
        Dim oldEditRange As Nullable(Of Integer) = Nothing
        If Me.LookUpEditRange.EditValue IsNot Nothing Then
            oldEditRange = Convert.ToInt32(Me.LookUpEditRange.EditValue)
        End If
        Dim dicEditRange As Dictionary(Of Integer, String) = Nothing
        'Dim h As New DateChooseHelper(_RangeFunction.ChooseKind)
        Select Case dateStartKind
            Case DateStartKind.Year, DateStartKind.Month, DateStartKind.Quarter
                dicEditRange = DateFunctionsControlData.GetThisAfterAgoData(dateStartKind)
        End Select
        If dicEditRange Is Nothing Then
            dicEditRange = DateFunctionsControlData.FullThisAfterAgo
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
        If _DateFunction IsNot Nothing Then
            Me.UpdateRangeCriteria()
        End If
        _CriteriaEditing = False
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

        Me.UpdateCriteria(dateChooseHelper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)

        'RaiseEvent OnRangeCriteriaChanged(dateRangeCriteria.UpdateRangeCriteria(_RangeFunction, Me.PropertyName))
        'Me.LabelCriteria.Text = dateRangeCriteria.RangeCriteria.ToString
        'Me.LabelCriteriaBetween.Text = dateRangeCriteria.RangeCriteriaBetweenText

    End Sub

    Public Overridable Sub UpdateCriteria(helper As DateChooseHelper, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
        Dim dateRangeCriteria As New DateCriteria(DateChooseHelper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)
        RaiseEvent OnCriteriaChanged(dateRangeCriteria.UpdateRangeCriteria(Me.DateFunction, Me.PropertyName))

    End Sub

    Private Sub SpinEditYear_EditValueChanged(sender As Object, e As EventArgs) Handles SpinEditYear.EditValueChanged, LookUpEditQuater.EditValueChanged, LookUpEditMonthWeek.EditValueChanged, LookUpEditMonth.EditValueChanged, LookUpEditFirstDayOfWeek.EditValueChanged, LookUpEditMonthAfter.EditValueChanged, CheckEditYear.CheckedChanged
        If _CriteriaEditing Then Return
        _CriteriaEditing = True
        If sender Is Me.LookUpEditMonth Then
            Me.UpdateAfterAgo(DateStartKind.Month)
        ElseIf sender Is Me.LookUpEditQuater Then
            Me.UpdateAfterAgo(DateStartKind.Quarter)
        End If
        If Me.LookUpEditRange.EditValue IsNot Nothing AndAlso _DateFunction IsNot Nothing Then
            Me.UpdateRangeCriteria()
        End If

        _CriteriaEditing = False
    End Sub



    Private Sub LookUpEditRange_EditValueChanged(sender As Object, e As EventArgs) Handles LookUpEditRange.EditValueChanged
        If Me.LookUpEditRange.EditValue Is Nothing OrElse _DateFunction Is Nothing Then Return
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
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
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
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
            Else
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
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
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
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
                'Me.lciRangeOption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
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
                Me.GridLookUpEditRangeCriteria.Properties.DataSource = Me.DateCriteriaFunctionInfos
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
