
Imports System.Linq.Expressions
Imports System.Reflection
Imports DevExpress.Data.Filtering

Namespace AcurSoft.Data.Filtering
    Public Class DateChooseHelper
        'Public Const This As Integer = 1 << 100
        'Public Const Choose As Integer = 1 << 101
        'Public Const After As Integer = 1 << 102
        'Public Const Ago As Integer = 1 << 103

        Private _IsSetting As Boolean

        Private _DateChooseKind As DateChooseKind = DateChooseKind.None
        Public Property DateChooseKind As DateChooseKind
            Get
                Return _DateChooseKind
            End Get
            Set(value As DateChooseKind)
                _DateChooseKind = value
                If _IsSetting Then Return
                _IsSetting = True
                _Value = Convert.ToInt32(value Or _DateStartKind)
                _IsSetting = False
            End Set
        End Property

        Private _DateStartKind As DateStartKind = DateStartKind.None
        Public Property DateStartKind As DateStartKind
            Get
                Return _DateStartKind
            End Get
            Set(value As DateStartKind)
                _DateStartKind = value
                If _IsSetting Then Return
                _IsSetting = True
                _Value = Convert.ToInt32(value Or _DateChooseKind)
                _IsSetting = False
            End Set
        End Property

        Private _Value As Integer
        Public Property Value As Integer
            Get
                Return _Value
            End Get
            Set(value As Integer)
                _Value = value
                If _IsSetting Then Return
                _IsSetting = True
                If value.HasBitFlag(DateChooseKind.This) Then
                    _DateChooseKind = DateChooseKind.This
                ElseIf value.HasBitFlag(DateChooseKind.After) Then
                    _DateChooseKind = DateChooseKind.After
                ElseIf value.HasBitFlag(DateChooseKind.Ago) Then
                    _DateChooseKind = DateChooseKind.Ago
                ElseIf value.HasBitFlag(DateChooseKind.Ago) Then
                    _DateChooseKind = DateChooseKind.Ago
                ElseIf value.HasBitFlag(DateChooseKind.Choose) Then
                    _DateChooseKind = DateChooseKind.Choose
                Else
                    _DateChooseKind = DateChooseKind.None
                End If

                If value.HasBitFlag(DateStartKind.Month) Then
                    _DateStartKind = DateStartKind.Month
                ElseIf value.HasBitFlag(DateStartKind.Quarter) Then
                    _DateStartKind = DateStartKind.Quarter
                ElseIf value.HasBitFlag(DateStartKind.Year) Then
                    _DateStartKind = DateStartKind.Year
                Else
                    _DateStartKind = DateStartKind.None
                End If
                _IsSetting = False
            End Set
        End Property

        Public Function HasDatePart(dateStartKind As DateStartKind) As Boolean
            Return Me.Value.HasBitFlag(dateStartKind)
        End Function

        Public Property AfterAgoValue As Integer

        Public Sub New()
        End Sub
        Public Sub New(v As Integer)
            Me.Value = v
        End Sub

        Public Sub New(dateStartKind As DateStartKind, dateChooseKind As DateChooseKind)
            Me.DateStartKind = dateStartKind
            Me.DateChooseKind = dateChooseKind
        End Sub

        Public Shared Function Instance() As DateChooseHelper
            Return New DateChooseHelper()
        End Function

        Public Shared Function Instance(v As Integer) As DateChooseHelper
            Return New DateChooseHelper(v)
        End Function

        Public Shared Function Instance(dateStartKind As DateStartKind, dateChooseKind As DateChooseKind) As DateChooseHelper
            Return New DateChooseHelper(dateStartKind, dateChooseKind)
        End Function


    End Class

    <Flags>
    Public Enum MonthWeekKind
        FirstWeek = 1
        SecondWeek = 2
        ThirdWeek = 3
        LastWeek = -1
        FullWeek = 1000
        FirstFullWeek = FirstWeek + FullWeek
        SecondFullWeek = SecondWeek + FullWeek
        ThirdFullWeek = ThirdWeek + FullWeek
        LastFullWeek = LastWeek - FullWeek
    End Enum
    'With dicMonthWeek
    '.Add(1, "1st Week")
    '.Add(2, "2nd Week")
    '.Add(3, "3rd Week")
    '.Add(1001, "1st Full Week")
    '.Add(1002, "2nd Full Week")
    '.Add(1003, "3rd Full Week")
    '.Add(-1, "Last Week")
    '.Add(-1001, "Last Full Week")
    'End With



    <Flags>
    Public Enum DateChooseKind
        None = 0
        This = 1 << 100
        Choose = 1 << 101
        After = 1 << 102
        Ago = 1 << 103
    End Enum

    <Flags>
    Public Enum DateStartKind
        None = 0
        Unkown = -1
        Year = 1 << 5001
        Month = 1 << 5002
        MonthWeekNum = 1 << 5003
        Week = 1 << 5004
        Quarter = 1 << 5005
        Semeseter = 1 << 5006

        'This = DateChooseKind.This
        'Choose = DateChooseKind.Choose
        'After = DateChooseKind.After
        'Ago = DateChooseKind.Ago
        'ThisYear = Year Or This
        'SelectYear = Year Or Choose
        'ThisMonth
        'SelectMonth
        'ThisWeek
        'SelectWeek
        'ThisQuarter
        'SelectQuarter
        'ThisSemester
        'SelectSemester
    End Enum

    <Flags>
    Public Enum DateFunctionKind
        None = 0
        IsDate = 1 << 1
        InRange = 1 << 2
        IsInteger = 1 << 3
        NeedNone = 1 << 5
        NeedYear = DateStartKind.Year
        NeedQuarter = DateStartKind.Quarter
        NeedMonth = DateStartKind.Month
        'NeedWeekNumber = 1 << 7
        NeedWeekNumber = 1 << 7
        NeedDays = 1 << 50
        NeedMonthWeekNumber = DateStartKind.Month Or DateStartKind.MonthWeekNum
        NeedDate = 1 << 8
        IsYear = 1 << 9
        'GetDate = IsDate Or NeedNone
        GetRange = InRange Or NeedDate
        GetRangeNeedYear = InRange Or NeedDate Or NeedYear
        GetRangeNeedYearMonth = GetRangeNeedYear Or DateFunctionKind.NeedMonth
        GetRangeNeedYearQuarter = GetRangeNeedYear Or DateFunctionKind.NeedQuarter
        GetRangeNeedYearMonthWeekNumber = GetRangeNeedYearMonth Or DateFunctionKind.NeedMonthWeekNumber

    End Enum

    Public Class XpoFunctionForDates
        Inherits XpoFunctionBase

        Public Overridable Property Kind As DateFunctionKind = DateFunctionKind.IsDate

        Public Sub New(mi As MethodInfo)
            Me.New(mi, mi.Name)
        End Sub
        Public Sub New(d As [Delegate], functionName As String)
            MyBase.New(d, functionName)
        End Sub

        Public Sub New(exp As LambdaExpression, functionName As String)
            MyBase.New(exp, functionName)
        End Sub
        Public Sub New(mi As MethodInfo, functionName As String)
            MyBase.New(mi, functionName)
        End Sub

        Public Function ToCriteriaEx(ParamArray cr As CriteriaOperator()) As CriteriaOperator
            Return New FunctionOperator(Me.Name, cr)
        End Function

        Public ReadOnly Property DateStartKind As DateStartKind
            Get
                Return DateChooseHelper.Instance(Convert.ToInt32(Me.Kind)).DateStartKind
            End Get
        End Property

        Public ReadOnly Property NeedMonthWeekNumber As Boolean
            Get
                'Return Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber)
                Return Me.Kind.HasBitFlag(DateStartKind.MonthWeekNum)
            End Get
        End Property

        Public Function ToCriteria() As CriteriaOperator
            If Me.Kind = DateFunctionKind.IsDate OrElse Me.Kind = DateFunctionKind.GetRange Then
                Return New FunctionOperator(Me.Name)
            ElseIf Me.Kind.HasFlag(DateFunctionKind.GetRange)
                If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue, New OperandValue, New OperandValue, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue)
                End If
            ElseIf Me.Kind.HasFlag(DateFunctionKind.IsDate)
                If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue, New OperandValue, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
                    Return New FunctionOperator(Me.Name, New OperandValue, New OperandValue)
                ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
                    Return New FunctionOperator(Me.Name, New OperandValue)
                End If
            End If
            Return Nothing
        End Function

        Private _DefaultParametersCriteria As List(Of CriteriaOperator)
        Public Overrides Property DefaultParametersCriteria As List(Of CriteriaOperator)
            Get
                If _DefaultParametersCriteria Is Nothing Then
                    _DefaultParametersCriteria = MyBase.DefaultParametersCriteria
                    If Me.Kind = DateFunctionKind.IsDate OrElse Me.Kind = DateFunctionKind.GetRange Then
                    ElseIf Me.Kind.HasFlag(DateFunctionKind.GetRange)
                        If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Month))
                            _DefaultParametersCriteria.Add(New OperandValue(1))
                            _DefaultParametersCriteria.Add(New OperandValue(False))
                            _DefaultParametersCriteria.Add(New OperandValue(Convert.ToInt32(DayOfWeek.Monday)))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Month))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(1))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                        End If
                    ElseIf Me.Kind.HasFlag(DateFunctionKind.IsDate)
                        If Me.Kind.HasFlag(DateFunctionKind.NeedMonthWeekNumber) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Month))
                            _DefaultParametersCriteria.Add(New OperandValue(1))
                            _DefaultParametersCriteria.Add(New OperandValue(False))
                            _DefaultParametersCriteria.Add(New OperandValue(Convert.ToInt32(DayOfWeek.Monday)))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedMonth) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Month))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedQuarter) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                            _DefaultParametersCriteria.Add(New OperandValue(1))
                        ElseIf Me.Kind.HasFlag(DateFunctionKind.NeedYear) Then
                            _DefaultParametersCriteria.Add(New OperandValue(Date.Today.Year))
                        End If
                    End If
                End If
                Return _DefaultParametersCriteria
            End Get
            Set(value As List(Of CriteriaOperator))
                _DefaultParametersCriteria = value
            End Set
        End Property



        Public Overloads Function GetInfo() As DateCriteriaFunctionInfo
            Return New DateCriteriaFunctionInfo(Me)
        End Function

    End Class
End Namespace
