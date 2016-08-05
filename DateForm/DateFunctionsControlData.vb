Imports AcurSoft.Data.Filtering

Public Class DateFunctionsControlData

    Private Shared _QuaterMonthChoiceData As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property QuaterMonthChoiceData As Dictionary(Of Integer, String)
        Get
            If _QuaterMonthChoiceData Is Nothing Then
                _QuaterMonthChoiceData = New Dictionary(Of Integer, String)
                With _QuaterMonthChoiceData
                    .Add(DateStartKind.Unkown, "")
                    .Add(DateStartKind.Month, "Month")
                    .Add(DateStartKind.Quarter, "Quarter")
                End With
            End If
            Return _QuaterMonthChoiceData
        End Get
    End Property

    Private Shared _ThisAfterAgo As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property ThisAfterAgo As Dictionary(Of Integer, String)
        Get
            If _ThisAfterAgo Is Nothing Then
                _ThisAfterAgo = New Dictionary(Of Integer, String)
                With _ThisAfterAgo
                    .Add(0, "This")
                    .Add(1000, "After")
                    .Add(-1000, "Ago")
                End With
            End If
            Return _ThisAfterAgo
        End Get
    End Property

    Private Shared _Quarters As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property Quarters As Dictionary(Of Integer, String)
        Get
            If _Quarters Is Nothing Then
                _Quarters = DateFunctionsControlData.ThisAfterAgo.ToDictionary(Function(q) q.Key, Function(q) q.Value)
                For i As Integer = 1 To 4
                    _Quarters.Add(i, "Q" & i)
                Next
            End If
            Return _Quarters
        End Get
    End Property

    Private Shared _Months As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property Months As Dictionary(Of Integer, String)
        Get
            If _Months Is Nothing Then
                _Months = DateFunctionsControlData.ThisAfterAgo.ToDictionary(Function(q) q.Key, Function(q) q.Value)
                Dim dateFoo As New Date(2016, 1, 1)
                For i As Integer = 1 To 12
                    _Months.Add(i, String.Format("{0:d2}: {1}", i, Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(dateFoo.AddMonths(i - 1).ToString("MMMM"))))
                Next
            End If
            Return _Months
        End Get
    End Property

    Private Shared _FirstDayOfWeeks As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property FirstDayOfWeeks As Dictionary(Of Integer, String)
        Get
            If _FirstDayOfWeeks Is Nothing Then
                _FirstDayOfWeeks = New Dictionary(Of Integer, String)
                For i As Integer = 0 To 6
                    _FirstDayOfWeeks.Add(i, DirectCast(i, DayOfWeek).ToString)
                Next
            End If
            Return _FirstDayOfWeeks
        End Get
    End Property
    Private Shared _MonthWeeks As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property MonthWeeks As Dictionary(Of Integer, String)
        Get
            If _MonthWeeks Is Nothing Then
                _MonthWeeks = New Dictionary(Of Integer, String)
                With _MonthWeeks
                    .Add(MonthWeekKind.FirstWeek, "1st Week")
                    .Add(MonthWeekKind.SecondWeek, "2nd Week")
                    .Add(MonthWeekKind.ThirdWeek, "3rd Week")
                    .Add(MonthWeekKind.FirstFullWeek, "1st Full Week")
                    .Add(MonthWeekKind.SecondFullWeek, "2nd Full Week")
                    .Add(MonthWeekKind.ThirdFullWeek, "3rd Full Week")
                    .Add(MonthWeekKind.LastWeek, "Last Week")
                    .Add(MonthWeekKind.LastFullWeek, "Last Full Week")
                End With
            End If
            Return _MonthWeeks
        End Get
    End Property


    Private Shared _FullThisAfterAgo As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property FullThisAfterAgo As Dictionary(Of Integer, String)
        Get
            If _FullThisAfterAgo Is Nothing Then

                _FullThisAfterAgo = New Dictionary(Of Integer, String)
                For Each kv In DateFunctionsControlData.YearThisAfterAgo
                    _FullThisAfterAgo.Add(kv.Key, kv.Value)
                Next
                For Each kv In DateFunctionsControlData.MonthThisAfterAgo
                    _FullThisAfterAgo.Add(kv.Key, kv.Value)
                Next
                For Each kv In DateFunctionsControlData.QuarterThisAfterAgo
                    _FullThisAfterAgo.Add(kv.Key, kv.Value)
                Next
            End If
            Return _FullThisAfterAgo
        End Get
    End Property

    Private Shared _YearThisAfterAgo As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property YearThisAfterAgo As Dictionary(Of Integer, String)
        Get
            If _YearThisAfterAgo Is Nothing Then
                _YearThisAfterAgo = New Dictionary(Of Integer, String)
                With _YearThisAfterAgo
                    .Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.This).Value, "This Year")
                    .Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.Choose).Value, "Year")
                    .Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.After).Value, "After N Year(s)")
                    .Add(DateChooseHelper.Instance(DateStartKind.Year, DateChooseKind.Ago).Value, "N Year(s) Ago")
                End With
            End If
            Return _YearThisAfterAgo
        End Get
    End Property

    Private Shared _MonthThisAfterAgo As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property MonthThisAfterAgo As Dictionary(Of Integer, String)
        Get
            If _MonthThisAfterAgo Is Nothing Then

                _MonthThisAfterAgo = New Dictionary(Of Integer, String)
                With _MonthThisAfterAgo
                    .Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.This).Value, "This Month")
                    .Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.Choose).Value, "Month")
                    .Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.After).Value, "After N Month(s)")
                    .Add(DateChooseHelper.Instance(DateStartKind.Month, DateChooseKind.Ago).Value, "N Month(s) Ago")
                End With

            End If
            Return _MonthThisAfterAgo
        End Get
    End Property

    Private Shared _QuarterThisAfterAgo As Dictionary(Of Integer, String)
    Public Shared ReadOnly Property QuarterThisAfterAgo As Dictionary(Of Integer, String)
        Get
            If _QuarterThisAfterAgo Is Nothing Then

                _QuarterThisAfterAgo = New Dictionary(Of Integer, String)
                With _QuarterThisAfterAgo
                    .Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.This).Value, "This Quarter")
                    .Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.Choose).Value, "Quarter")
                    .Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.After).Value, "After N Quarter(s)")
                    .Add(DateChooseHelper.Instance(DateStartKind.Quarter, DateChooseKind.Ago).Value, "N Quarter(s) Ago")
                End With
            End If
            Return _QuarterThisAfterAgo
        End Get
    End Property

    Public Shared Function GetThisAfterAgoData(dateStartKind As DateStartKind) As Dictionary(Of Integer, String)
        Select Case dateStartKind
            Case DateStartKind.Year
                Return DateFunctionsControlData.YearThisAfterAgo
            Case DateStartKind.Month
                Return DateFunctionsControlData.MonthThisAfterAgo
            Case DateStartKind.Quarter
                Return DateFunctionsControlData.QuarterThisAfterAgo
        End Select
        Return Nothing
    End Function


    'dicEditRange = DateFunctionsControlData.DicRangeStart.Where(Function(q) New DateChooseHelper(q.Key).DateStartKind = dateStartKind).ToDictionary(Function(q) q.Key, Function(q) q.Value)



    Public Sub New()
    End Sub

End Class
