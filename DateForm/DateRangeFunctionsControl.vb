Imports DevExpress.Data.Filtering
Imports AcurSoft.Data.Filtering

Public Class DateRangeFunctionsControl
    Inherits DateFunctionsControl
    Public Event OnRangeCriteriaChanged(e As DateRangeCriteria)

    Public ReadOnly Property RangeFunction As XpoFunctionForDateRanges
        Get
            Return DirectCast(Me.DateFunction, XpoFunctionForDateRanges)
        End Get
    End Property

    Public Overrides Function GetDateCriteriaFunctionInfos() As List(Of DateCriteriaFunctionInfo)
        Return CriteriaOperator.GetCustomFunctions.OfType(Of XpoFunctionForDateRanges).Select(Function(q) q.GetInfo()).ToList()
    End Function


    Public Overrides Sub UpdateCriteria(helper As DateChooseHelper, year As Integer, quarter As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
        Dim dateRangeCriteria As New DateRangeCriteria(DateChooseHelper, year, quarter, month, monthWeekNumber, fullWeek, firstDayOfWeek)

        RaiseEvent OnRangeCriteriaChanged(DirectCast(dateRangeCriteria.UpdateRangeCriteria(Me.RangeFunction, Me.PropertyName), DateRangeCriteria))

    End Sub
End Class
