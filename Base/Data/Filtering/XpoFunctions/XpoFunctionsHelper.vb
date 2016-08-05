Imports System.Linq.Expressions
Imports System.Reflection
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Xpo
Namespace AcurSoft.Data.Filtering

    Public Class XpoFunctionsHelper

        Private Shared _Instance As XpoFunctionsHelper
        Public Shared ReadOnly Property Instance As XpoFunctionsHelper
            Get
                If _Instance Is Nothing Then
                    _Instance = New XpoFunctionsHelper
                End If
                Return _Instance
            End Get
        End Property



        'Private Shared _CriteriaFunctionInfos As Dictionary(Of String, CriteriaFunctionBaseInfo)
        'Public Shared ReadOnly Property CriteriaFunctionInfos As Dictionary(Of String, CriteriaFunctionBaseInfo)
        '    Get
        '        If _CriteriaFunctionInfos Is Nothing Then
        '            _CriteriaFunctionInfos = New Dictionary(Of String, CriteriaFunctionBaseInfo)
        '        End If
        '        Return _CriteriaFunctionInfos
        '    End Get
        'End Property

#Region "Shared"

        Public Shared Sub UnRegisterFunction(functionName As String)
            Dim oldFunction As ICustomFunctionOperator = CriteriaOperator.GetCustomFunction(functionName)
            If oldFunction IsNot Nothing Then
                CriteriaOperator.UnregisterCustomFunction(oldFunction)
            End If
            'CustomCriteriaManager.
        End Sub

        Public Shared Function Register(Of F As ICustomFunctionOperator)(fnc As F) As F
            If DirectCast(CriteriaOperator.GetCustomFunction(fnc.Name), F) Is Nothing Then
                CriteriaOperator.RegisterCustomFunction(fnc)
                If TypeOf fnc Is ICustomCriteriaOperatorQueryable Then
                    CustomCriteriaManager.RegisterCriterion(DirectCast(fnc, ICustomCriteriaOperatorQueryable))
                End If
                'If Not RegistredFunctions.ContainsKey(fnc.Name) Then
                '    RegistredFunctions.Add(fnc.Name, fnc)
                'End If
            End If
            Return fnc
        End Function

        Public Shared Function Register(Of F As ICustomFunctionOperator)(functionName As String, ParamArray args As Object()) As F
            UnRegisterFunction(functionName)
            Dim fnc As F
            If args Is Nothing OrElse args.Count = 0 Then
                fnc = Activator.CreateInstance(Of F)
            Else
                fnc = DirectCast(Activator.CreateInstance(GetType(F), args), F)
            End If
            fnc.UnSafeRegister()
            Return fnc
        End Function


        'Public Shared Function Register(Of F As ICustomFunctionOperator)(functionName As String, mi As MethodInfo) As F
        '    UnRegisterFunction(functionName)
        '    Dim fnc As F
        '    Dim params As New List(Of Object)
        '    params.Add(mi)
        '    params.Add(functionName)
        '    fnc = DirectCast(Activator.CreateInstance(GetType(F), params.ToArray), F)
        '    fnc.UnSafeRegister()
        '    Return fnc
        'End Function

        'Public Shared Function Register(Of F As ICustomFunctionOperator)(functionName As String, exp As LambdaExpression) As F
        '    UnRegisterFunction(functionName)
        '    Dim fnc As F
        '    Dim params As New List(Of Object)
        '    params.Add(exp)
        '    params.Add(functionName)
        '    fnc = DirectCast(Activator.CreateInstance(GetType(F), params.ToArray), F)
        '    fnc.UnSafeRegister()
        '    Return fnc
        'End Function


        Public Shared Function RegisterMethod(Of F As XpoFunctionBase)(functionName As String, mi As MethodInfo) As F
            UnRegisterFunction(functionName)
            Dim fnc As F
            fnc = DirectCast(Activator.CreateInstance(GetType(F), mi, functionName), F)
            fnc.SafeRegister()
            Return fnc
        End Function

        Public Shared Function RegisterExpression(Of F As XpoFunctionBase)(functionName As String, exp As LambdaExpression) As F
            UnRegisterFunction(functionName)
            Dim fnc As F
            fnc = DirectCast(Activator.CreateInstance(GetType(F), exp, functionName), F)
            fnc.SafeRegister()
            Return fnc
        End Function

        Public Shared Function RegisterDelegate(Of F As XpoFunctionBase)(functionName As String, delegateEvaluator As [Delegate]) As F
            UnRegisterFunction(functionName)
            Dim fnc As F
            fnc = DirectCast(Activator.CreateInstance(GetType(F), delegateEvaluator, functionName), F)
            fnc.SafeRegister()
            Return fnc
        End Function
        'Public Shared Function GetStoredCriteriaFunctionInfo(id As Integer) As CriteriaFunctionBaseInfo
        '    Return XpoFunctionsHelper.CriteriaFunctionInfos.Values.FirstOrDefault(Function(q) q.Id = id)
        'End Function

        'Public Shared Function StoreCriteriaFunctionInfo(fnc As XpoFunctionBase) As CriteriaFunctionBaseInfo
        '    Dim info As CriteriaFunctionBaseInfo = Nothing
        '    'Dim id As Integer = 0
        '    'If XpoFunctionsHelper.CriteriaFunctionInfos.ContainsKey(fnc.Name) Then
        '    '    id = XpoFunctionsHelper.CriteriaFunctionInfos(fnc.Name).Id
        '    '    XpoFunctionsHelper.CriteriaFunctionInfos.Remove(fnc.Name)
        '    'End If
        '    'info = fnc.GetInfo()
        '    'If id = 0 Then
        '    '    'id = _ClauseTypeBaseValue
        '    '    'If id = 0 Then
        '    '    '    id = XpoFunctionsHelper.ClauseTypeBaseValue
        '    '    'End If
        '    '    id = XpoFunctionsHelper.ClauseTypeBaseValue
        '    '    _ClauseTypeBaseValue += 1
        '    'End If
        '    'info.Id = id
        '    'XpoFunctionsHelper.CriteriaFunctionInfos.Add(fnc.Name, info)

        '    Return info
        'End Function

        Public Shared Function RegisterDateDelegate(
                       functionName As String,
                       delegateEvaluator As [Delegate],
                       kind As DateFunctionKind) As XpoFunctionForDates
            Dim fnc As XpoFunctionForDates = RegisterDelegate(Of XpoFunctionForDates)(functionName, delegateEvaluator)
            fnc.Kind = kind
            'If store Then
            '    StoreCriteriaFunctionInfo(fnc)
            'End If

            Return fnc
        End Function

        Public Shared Function RegisterDateRangeDelegate(
                       functionName As String,
                       delegateEvaluator As [Delegate],
                       kind As DateFunctionKind,
                       startFunction As XpoFunctionForDates,
                       endFunction As XpoFunctionForDates) As XpoFunctionForDateRanges
            Dim fnc As XpoFunctionForDateRanges = RegisterDelegate(Of XpoFunctionForDateRanges)(functionName, delegateEvaluator)
            fnc.Kind = kind
            fnc.StartFunction = startFunction
            fnc.EndFunction = endFunction

            'If store Then
            '    StoreCriteriaFunctionInfo(fnc)
            'End If

            Return fnc
        End Function
        'Public Shared Function Register(Of F As XpoFunctionBase)(functionName As String, exp As LambdaExpression) As F
        '    UnRegisterFunction(functionName)
        '    Dim fnc As F
        '    Dim params As New List(Of Object)
        '    params.Add(exp)
        '    params.Add(functionName)
        '    fnc = DirectCast(Activator.CreateInstance(GetType(F), params.ToArray), F)
        '    fnc.UnSafeRegister()
        '    Return fnc
        'End Function
#End Region

#Region "SharedHelpers"

        Public Shared Function MethodName(ByVal expression As LambdaExpression) As MethodInfo
            Dim unaryExpression = CType(expression.Body, UnaryExpression)
            Dim methodCallExpression = CType(unaryExpression.Operand, MethodCallExpression)
            Dim methodCallObject = CType(methodCallExpression.Object, ConstantExpression)
            Dim methodInfo = CType(methodCallObject.Value, MethodInfo)

            Return methodInfo
        End Function
#End Region
        Public Shared Sub RegisterDateRangesFunctions()
            Dim YearStart As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearStart",
                Function(year As Integer) New Date(year, 1, 1),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear)

            Dim YearEnd As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearEnd",
                Function(year As Integer) New Date(year + 1, 1, 1).AddDays(-1),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear)

            Dim InYear As XpoFunctionForDateRanges = RegisterDateRangeDelegate(
                "InYear",
                Function(d As Date, year As Integer)
                    Dim d1 As New Date(year, 1, 1)
                    Dim d2 As Date = d1.AddYears(1).AddDays(-1)
                    Return d >= d1 AndAlso d <= d2
                End Function,
                DateFunctionKind.GetRange Or DateFunctionKind.NeedYear,
                YearStart,
                YearEnd)

            'InYear.Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedYear
            'InYear.StartFunction = YearStart
            'InYear.EndFunction = YearEnd


            Dim YearMonthStart As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearMonthStart",
                Function(year As Integer, month As Integer) New Date(year, 1, 1).AddMonths(month - 1),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonth)

            Dim YearMonthEnd As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearMonthEnd",
                Function(year As Integer, month As Integer) New Date(year, 1, 1).AddMonths(month).AddDays(-1),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonth)

            'Dim InYearMonth As XpoFunctionForDateRanges = RegisterDelegate(Of XpoFunctionForDateRanges)(
            '    "InYearMonth",
            '    Function(d As Date, year As Integer, month As Integer)
            '        Dim d1 As Date = New Date(year, 1, 1).AddMonths(month - 1)
            '        Dim d2 As Date = d1.AddMonths(1).AddDays(-1)
            '        Return d >= d1 AndAlso d <= d2
            '    End Function)
            'InYearMonth.Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonth
            'InYearMonth.StartFunction = YearMonthStart
            'InYearMonth.EndFunction = YearMonthEnd

            Dim InYearMonth As XpoFunctionForDateRanges = RegisterDateRangeDelegate(
                "InYearMonth",
                Function(d As Date, year As Integer, month As Integer)
                    Dim d1 As Date = New Date(year, 1, 1).AddMonths(month - 1)
                    Dim d2 As Date = d1.AddMonths(1).AddDays(-1)
                    Return d >= d1 AndAlso d <= d2
                End Function,
                DateFunctionKind.GetRange Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonth,
                YearMonthStart,
                YearMonthEnd)



            Dim YearQuarterStart As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearQuarterStart",
                Function(year As Integer, quarter As Integer) New Date(year, 1, 1).AddMonths((quarter - 1) * 3),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedQuarter)

            Dim YearQuarterEnd As XpoFunctionForDates = RegisterDateDelegate(
                "GetYearQuarterEnd",
                Function(year As Integer, quarter As Integer) New Date(year, 1, 1).AddMonths(3 + ((quarter - 1) * 3)).AddDays(-1),
                DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedQuarter)

            Dim InYearQuarter As XpoFunctionForDateRanges = RegisterDateRangeDelegate(
                "InYearQuarter",
                Function(d As Date, year As Integer, quarter As Integer)
                    Dim d1 As Date = New Date(year, 1, 1).AddMonths((quarter - 1) * 3)
                    Dim d2 As Date = d1.AddMonths(3).AddDays(-1)
                    Return d >= d1 AndAlso d <= d2
                End Function,
                DateFunctionKind.GetRange Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedQuarter,
                YearQuarterStart,
                YearQuarterEnd)




            Dim YearMonthWeekStart As XpoFunctionForDates = RegisterDelegate(Of XpoFunctionForDates)(
                "GetYearMonthWeekStart", Function(year As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
                                             If monthWeekNumber = -1 Then
                                                 'Dim lastDayOfMonth As Date = New Date(year, month, 1).AddMonths(1).AddDays(-1)
                                                 'Return lastDayOfMonth.AddDays(-((CInt(lastDayOfMonth.DayOfWeek) - firstDayOfWeek + 7) Mod 7))
                                                 Return New Date(year, 1, 1).AddMonths(month - 1).LastDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek)
                                             Else
                                                 'Dim firstMonthDay As New Date(year, 1, 1).
                                                 Dim rtn As Date = New Date(year, 1, 1).AddMonths(month - 1).FirstDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek)
                                                 'rtn = rtn.AddDays((month - rtn.Month) * 7).AddDays((weekNum - 1) * 7)
                                                 Return rtn
                                             End If
                                         End Function)
            YearMonthWeekStart.Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonthWeekNumber

            Dim YearMonthWeekEnd As XpoFunctionForDates = RegisterDelegate(Of XpoFunctionForDates)(
                "GetYearMonthWeekEnd", Function(year As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
                                           If monthWeekNumber = -1 Then
                                               'Dim lastDayOfMonth As Date = New Date(year, month, 1).AddMonths(1).AddDays(-1)
                                               'Return lastDayOfMonth.AddDays(-((CInt(lastDayOfMonth.DayOfWeek) - firstDayOfWeek + 7) Mod 7)).AddDays(6)
                                               Return New Date(year, 1, 1).AddMonths(month - 1).LastDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek).AddDays(6)
                                           Else
                                               'Dim firstMonthDay As New Date(year, month, 1)
                                               Dim rtn As Date = New Date(year, 1, 1).AddMonths(month - 1).FirstDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek)
                                               'rtn = rtn.AddDays((month - rtn.Month) * 7).AddDays((weekNum - 1) * 7)
                                               Return rtn.AddDays(6)
                                           End If
                                       End Function)
            YearMonthWeekEnd.Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonthWeekNumber


            Dim InYearMonthWeek As XpoFunctionForDateRanges = RegisterDelegate(Of XpoFunctionForDateRanges)("InYearMonthWeek",
                 Function(d As Date, year As Integer, month As Integer, monthWeekNumber As Integer, fullWeek As Boolean, firstDayOfWeek As Integer)
                     Dim d1 As Date = Nothing
                     Dim d2 As Date = Nothing
                     Dim firstMonthDay As Date = New Date(year, 1, 1).AddMonths(month - 1)
                     If monthWeekNumber = -1 Then
                         d1 = firstMonthDay.LastDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek)
                     Else
                         d1 = firstMonthDay.FirstDayOfWeekInMonth(DirectCast(firstDayOfWeek, DayOfWeek), fullWeek).AddDays((monthWeekNumber - 1) * 7)
                     End If
                     d2 = d1.AddDays(6)
                     Return d >= d1 AndAlso d <= d2
                 End Function)
            InYearMonthWeek.Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedMonthWeekNumber
            InYearMonthWeek.StartFunction = YearMonthWeekStart
            InYearMonthWeek.EndFunction = YearMonthWeekEnd
        End Sub

        Public Shared Sub RegisterDatesFunctions()
            'RegisterDelegate(Of XpoFunctionForDates)("ThisYearFirstDay", Function() New Date(Date.Today.Year, 1, 1))
            'RegisterDelegate(Of XpoFunctionForDates)("ThisYearLastDay", Function() New Date(Date.Today.Year + 1, 1, 1).AddDays(-1))
            'RegisterDelegate(Of XpoFunctionForDates)("InThisYear", Function(d As Date)
            '                                                           Dim d1 As New Date(Date.Today.Year, 1, 1)
            '                                                           Dim d2 As Date = d1.AddYears(1).AddDays(-1)
            '                                                           Return d >= d1 AndAlso d <= d2
            '                                                       End Function).Kind = DateFunctionKind.GetRange
            RegisterDelegate(Of XpoFunctionForDates)("XToday", Function() New Date(2015, 1, 1))
            RegisterDelegate(Of XpoFunctionForDates)("Today", Function() Date.Today)
            RegisterDelegate(Of XpoFunctionForDates)("ThisYear", Function() Date.Today.Year).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("ThisMonth", Function() Date.Today.Month).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("ThisDay", Function() Date.Today.Day).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("ThisQuarter", Function() Date.Today.GetQuarter()).Kind = DateFunctionKind.IsInteger

            'RegisterDelegate(Of XpoFunctionForDates)("AfterDays", Function(days As Integer) Date.Today.(days)).Kind = DateFunctionKind.IsInteger ' Or DateFunctionKind.NeedDays
            'RegisterDelegate(Of XpoFunctionForDates)("AfterYears", Function(years As Integer) Date.Today.Year + years).Kind = DateFunctionKind.IsDate
            'RegisterDelegate(Of XpoFunctionForDates)("AfterMonths", Function(months As Integer) Date.Today.AddMonths(months)).Kind = DateFunctionKind.IsDate
            'RegisterDelegate(Of XpoFunctionForDates)("AfterQuarters", Function(quarters As Integer) Date.Today.FirstDayOfQuarter.AddMonths(quarters * 3)).Kind = DateFunctionKind.IsInteger

            'RegisterDelegate(Of XpoFunctionForDates)("AfterDays", Function(days As Integer) Date.Today.AddDays(days)).Kind = DateFunctionKind.IsDate ' Or DateFunctionKind.NeedDays
            'RegisterDelegate(Of XpoFunctionForDates)("AfterYears", Function(years As Integer) Date.Today.AddYears(years)).Kind = DateFunctionKind.IsDate
            'RegisterDelegate(Of XpoFunctionForDates)("AfterMonths", Function(months As Integer) Date.Today.AddMonths(months)).Kind = DateFunctionKind.IsDate
            'RegisterDelegate(Of XpoFunctionForDates)("AfterQuarters", Function(quarters As Integer) Date.Today.FirstDayOfQuarter.AddMonths(quarters * 3)).Kind = DateFunctionKind.IsInteger

            RegisterDelegate(Of XpoFunctionForDates)("XGetDate", Function(year As Integer, month As Integer, day As Integer) New Date(year, month, day))

            RegisterDelegate(Of XpoFunctionForDates)("GetQuarter", Function(d As Date) d.GetQuarter()).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("XGetYear", Function(d As Date) d.Year).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("XGetMonth", Function(d As Date) d.Month).Kind = DateFunctionKind.IsInteger
            RegisterDelegate(Of XpoFunctionForDates)("GetDay", Function(d As Date) d.Day).Kind = DateFunctionKind.IsInteger











            'RegisterDelegate(Of XpoFunctionForDates)("InThisYearMonth", Function(d As Date, month As Integer)
            '                                                                Dim d1 As New Date(Date.Today.Year, month, 1)
            '                                                                Dim d2 As Date = d1.AddDays(-1)
            '                                                                Return d >= d1 AndAlso d <= d2
            '                                                            End Function).Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedMonth



            'RegisterDelegate(Of XpoFunctionForDates)("NextYearFirstDay", Function() New Date(Date.Today.Year + 1, 1, 1))
            'RegisterDelegate(Of XpoFunctionForDates)("NextYearLastDay", Function() New Date(Date.Today.Year + 2, 1, 1).AddDays(-1))
            ''RegisterDelegate(Of XpoFunctionForDates)("InNextYear", Function(d As Date)
            ''                                                           Dim d1 As New Date(Date.Today.Year + 1, 1, 1)
            ''                                                           Dim d2 As Date = New Date(Date.Today.Year + 2, 1, 1).AddDays(-1)
            ''                                                           Return d >= d1 AndAlso d <= d2
            ''                                                       End Function).Kind = DateFunctionKind.GetRange



            'RegisterDelegate(Of XpoFunctionForDates)("ThisMonthFirstDay", Function() New Date(Date.Today.Year, Date.Today.Month, 1))
            'RegisterDelegate(Of XpoFunctionForDates)("ThisMonthLastDay", Function() New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(1).AddDays(-1))
            ''RegisterDelegate(Of XpoFunctionForDates)("InThisMonth", Function(d As Date)
            ''                                                            Dim d1 As New Date(Date.Today.Year, Date.Today.Month, 1)
            ''                                                            Dim d2 As Date = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(1).AddDays(-1)
            ''                                                            Return d >= d1 AndAlso d <= d2
            ''                                                        End Function).Kind = DateFunctionKind.GetRange




            'RegisterDelegate(Of XpoFunctionForDates)("NextMonthFirstDay", Function() New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(1))
            'RegisterDelegate(Of XpoFunctionForDates)("NextMonthLastDay", Function() New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(2).AddDays(-1))
            ''RegisterDelegate(Of XpoFunctionForDates)("InNextMonth", Function(d As Date)
            ''                                                            Dim d1 As Date = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(1)
            ''                                                            Dim d2 As Date = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(2).AddDays(-1)
            ''                                                            Return d >= d1 AndAlso d <= d2
            ''                                                        End Function).Kind = DateFunctionKind.GetRange

            'Dim dtFoo As New Date(2016, 1, 1)

            'For i As Integer = 0 To 11
            '    Dim monthNum As Integer = 1 + i
            '    Dim monthName As String = dtFoo.AddMonths(monthNum).ToString("MMMM", Globalization.CultureInfo.InvariantCulture)
            '    'Must use Linq Expression because in loop
            '    Dim expFirst As Expression(Of Func(Of Date)) = Function() New Date(Date.Today.Year, monthNum, 1)
            '    RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "FirstDay", expFirst)
            '    Dim expLast As Expression(Of Func(Of Date)) = Function() New Date(Date.Today.Year, monthNum, 1).AddMonths(1).AddDays(-1)
            '    RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "LastDay", expLast)
            '    Dim expIn As Expression(Of Func(Of Date, Boolean)) = Function(d As Date) d >= New Date(Date.Today.Year, monthNum, 1) AndAlso d <= New Date(Date.Today.Year, monthNum, 1).AddMonths(1).AddDays(-1)
            '    'RegisterExpression(Of XpoFunctionForDates)("InThis" & monthName, expIn).Kind = DateFunctionKind.GetRange
            'Next

            'For i As Integer = 0 To 11
            '    Dim monthNum As Integer = 1 + i
            '    Dim monthName As String = dtFoo.AddMonths(monthNum).ToString("MMMM", Globalization.CultureInfo.InvariantCulture)
            '    Dim dt As New Date(Date.Today.Year, monthNum, 1)
            '    For j As Integer = 0 To 6
            '        Dim weekDay As DayOfWeek = CType(j, DayOfWeek)
            '        Dim weekdayName As String = weekDay.ToString()

            '        Dim expFirst As Expression(Of Func(Of Date)) = Function() dt.FirstDayWeekOfMonth(weekDay)
            '        RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "First" & weekdayName, expFirst)

            '        Dim expSecond As Expression(Of Func(Of Date)) = Function() dt.SecondDayWeekOfMonth(weekDay)
            '        RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "Second" & weekdayName, expSecond)

            '        Dim expThird As Expression(Of Func(Of Date)) = Function() dt.ThirdDayWeekOfMonth(weekDay)
            '        RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "Third" & weekdayName, expThird)

            '        Dim expLast As Expression(Of Func(Of Date)) = Function() dt.LastDayWeekOfMonth(weekDay)
            '        RegisterExpression(Of XpoFunctionForDates)("This" & monthName & "Last" & weekdayName, expLast)
            '    Next
            'Next


            'For j As Integer = 0 To 6
            '    Dim weekDay As DayOfWeek = CType(j, DayOfWeek)
            '    Dim weekdayName As String = weekDay.ToString()
            '    Dim expThis As Expression(Of Func(Of Date)) = Function() Date.Today.DateOfWeek(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisWeek" & weekdayName, expThis)

            '    Dim expNext As Expression(Of Func(Of Date)) = Function() Date.Today.NextDateOfWeek(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisNext" & weekdayName, expNext)

            '    Dim expFirst As Expression(Of Func(Of Date)) = Function() Date.Today.FirstDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisMonthFirst" & weekdayName, expFirst)

            '    Dim expSecond As Expression(Of Func(Of Date)) = Function() Date.Today.SecondDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisMonthSecond" & weekdayName, expSecond)

            '    Dim expThird As Expression(Of Func(Of Date)) = Function() Date.Today.ThirdDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisMonthThird" & weekdayName, expThird)

            '    Dim expLast As Expression(Of Func(Of Date)) = Function() Date.Today.LastDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("ThisMonthLast" & weekdayName, expLast)
            'Next

            'For j As Integer = 0 To 6
            '    Dim weekDay As DayOfWeek = CType(j, DayOfWeek)
            '    Dim weekdayName As String = weekDay.ToString()
            '    Dim expThis As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.DateOfWeek(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("Get" & weekdayName, expThis).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate

            '    Dim expNext As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.NextDateOfWeek(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("GetNext" & weekdayName, expNext).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate

            '    Dim expFirst As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.FirstDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("GetMonthFirst" & weekdayName, expFirst).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate

            '    Dim expSecond As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.SecondDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("GetMonthSecond" & weekdayName, expSecond).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate

            '    Dim expThird As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.ThirdDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("GetMonthThird" & weekdayName, expThird).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate

            '    Dim expLast As Expression(Of Func(Of Date, Date)) = Function(d As Date) d.LastDayWeekOfMonth(weekDay)
            '    RegisterExpression(Of XpoFunctionForDates)("GetMonthLast" & weekdayName, expLast).Kind = DateFunctionKind.IsDate Or DateFunctionKind.NeedDate
            'Next

            'RegisterDelegate(Of XpoFunctionForDates)("ThisQuarterFirstDay", Function() Date.Today.FirstDayOfQuarter())
            'RegisterDelegate(Of XpoFunctionForDates)("ThisQuarterLastDay", Function() Date.Today.LastDayOfQuarter())
            ''RegisterDelegate(Of XpoFunctionForDates)("InThisQuarter", Function(d As Date) d >= Date.Today.FirstDayOfQuarter() AndAlso d <= Date.Today.LastDayOfQuarter()).Kind = DateFunctionKind.GetRange



            'For i As Integer = 0 To 3
            '    Dim qNum As Integer = i + 1
            '    Dim dt As New Date(Date.Today.Year, (qNum * 3) - 1, 1)
            '    Dim expFirst As Expression(Of Func(Of Date)) = Function() dt.FirstDayOfQuarter()
            '    RegisterExpression(Of XpoFunctionForDates)("ThisYearQ" & qNum & "FirstDay", expFirst)
            '    Dim expLast As Expression(Of Func(Of Date)) = Function() dt.LastDayOfQuarter()
            '    RegisterExpression(Of XpoFunctionForDates)("ThisYearQ" & qNum & "LastDay", expLast)
            '    Dim expIn As Expression(Of Func(Of Date, Boolean)) = Function(d As Date) d >= dt.FirstDayOfQuarter() AndAlso d <= dt.LastDayOfQuarter()
            '    'RegisterExpression(Of XpoFunctionForDates)("InThisYearQ" & qNum, expIn).Kind = DateFunctionKind.GetRange
            '    Dim expInEx As Expression(Of Func(Of Date, Integer, Boolean)) = Function(d As Date, y As Integer) d >= New Date(y, (qNum * 3) - 1, 1).FirstDayOfQuarter() AndAlso d <= New Date(y, (qNum * 3) - 1, 1).LastDayOfQuarter()
            '    'RegisterExpression(Of XpoFunctionForDates)("InYearQ" & qNum, expIn).Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedYear
            'Next
            'RegisterDelegate(Of XpoFunctionForDates)("InQuarter", Function(d As Date, y As Integer, q As Integer) d >= New Date(y, (q * 3) - 1, 1).FirstDayOfQuarter() AndAlso d <= New Date(y, (q * 3) - 1, 1).LastDayOfQuarter()).Kind = DateFunctionKind.GetRange Or DateFunctionKind.NeedYear Or DateFunctionKind.NeedQuarter



        End Sub

        Public Shared Sub Register()
            'Test
            CriteriaOperator.RegisterCustomFunction(New MatchesAnyOfFunction())

            'InAllowedBranchIdsFunction.Register()
            RegisterMethod(Of XpoFunctionBase)("GetTypeFullName", (Function(q As Type) q.FullName).Method)
            RegisterMethod(Of XpoFunctionBase)("MyTypeName", (Function(q As Object) q.GetType().FullName).Method)
            RegisterMethod(Of XpoFunctionBase)("MyType", (Function(q As Object) q.GetType()).Method)
            RegisterMethod(Of XpoFunctionBase)("HasBitFlag", (Function(i As Integer, flag As Integer) i.HasBitFlag(flag)).Method)

            RegisterDatesFunctions()
            RegisterDateRangesFunctions()
        End Sub



    End Class
End Namespace
