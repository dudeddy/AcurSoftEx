Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports AcurSoft.Data.Filtering.Helpers
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Xpo

Module Extensions

#Region "Filter"

    <Extension>
    Public Function AsOf(Of T As Structure)(ByVal o As [Enum]) As T
        Return CType(System.Enum.ToObject(GetType(T), Convert.ToInt32(o)), T)
    End Function

    <Extension>
    Public Function IsStandard(ByVal summaryItemType As AcurSoft.XtraGrid.Views.Grid.Extenders.SummaryItemTypeEx2) As Boolean
        Return System.Enum.IsDefined(GetType(SummaryItemType), summaryItemType.value__)
    End Function


    <Extension>
    Public Function IsDefined(ByVal clauseType As ClauseType) As Boolean
        Return System.Enum.IsDefined(GetType(ClauseType), clauseType)
    End Function

    <Extension>
    Public Function IntegerValue(ByVal clauseType As ClauseType) As Integer
        Return Convert.ToInt32(clauseType)
    End Function


#End Region
#Region "Dates"


    <Extension>
    Public Function FirstDayOfWeekInMonth(ByVal dt As Date, ByVal startOfWeek As DayOfWeek, fullWeek As Boolean) As Date
        Dim diff As Integer = dt.DayOfWeek - startOfWeek
        If diff <0 Then
            diff += 7
        End If
        Dim rtn As Date = dt.AddDays(-1 * diff).Date
        If fullWeek Then
            If rtn.Month = dt.Month Then
                Return rtn
            Else
                Return rtn.AddDays(7)
            End If
        Else
            Return rtn
        End If
    End Function


    <Extension>
    Public Function LastDayOfWeekInMonth(ByVal dt As Date, ByVal dayOfWeek As DayOfWeek, fullWeek As Boolean) As Date
        Dim lastDayOfMonth As Date = New Date(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1)
        Dim rtn As Date = lastDayOfMonth.AddDays(-((CInt(lastDayOfMonth.DayOfWeek) - CInt(dayOfWeek) + 7) Mod 7))
        If fullWeek Then
            If rtn.AddDays(6).Month = dt.Month Then
                Return rtn
            Else
                Return rtn.AddDays(-7)

            End If
        Else
            Return rtn
        End If
    End Function

    <Extension>
    Public Function DateOfWeek(ByVal dt As Date, ByVal startOfWeek As DayOfWeek) As Date
        Dim diff As Integer = dt.DayOfWeek - startOfWeek
        If diff < 0 Then
            diff += 7
        End If
        Return dt.AddDays(-1 * diff).Date
    End Function

    <Extension>
    Public Function NextDateOfWeek(ByVal dt As Date, ByVal startOfWeek As DayOfWeek) As Date
        Return dt.DateOfWeek(startOfWeek).AddDays(7)
    End Function

    <Extension>
    Public Function FirstDayWeekOfMonth(ByVal dt As Date, ByVal startOfWeek As DayOfWeek) As Date
        Dim firstMonthDay As New Date(dt.Year, dt.Month, 1)
        Dim rtn As Date = firstMonthDay.DateOfWeek(startOfWeek)
        rtn = rtn.AddDays((dt.Month - rtn.Month) * 7)
        Return rtn
    End Function

    <Extension>
    Public Function SecondDayWeekOfMonth(ByVal dt As Date, ByVal startOfWeek As DayOfWeek) As Date
        Return dt.FirstDayWeekOfMonth(startOfWeek).AddDays(7)
    End Function

    <Extension>
    Public Function ThirdDayWeekOfMonth(ByVal dt As Date, ByVal startOfWeek As DayOfWeek) As Date
        Return dt.FirstDayWeekOfMonth(startOfWeek).AddDays(7 * 2)
    End Function

    <Extension>
    Public Function LastDayWeekOfMonth(ByVal dt As Date, ByVal dayOfWeek As DayOfWeek) As Date
        Dim lastDayOfMonth As Date = New Date(dt.Year, dt.Month, 1).AddMonths(1).AddDays(-1)
        Return lastDayOfMonth.AddDays(-((CInt(lastDayOfMonth.DayOfWeek) - CInt(dayOfWeek) + 7) Mod 7))
    End Function

    <Extension>
    Public Function GetQuarter(ByVal dt As Date) As Integer
        Select Case dt.Month
            Case <= 3
                Return 1
            Case <= 6
                Return 2
            Case <= 9
                Return 3
        End Select
        Return 4
    End Function

    <Extension>
    Public Function GetFirstMonthOfQuarter(ByVal dt As Date) As Integer
        Select Case dt.Month
            Case <= 3
                Return 1
            Case <= 6
                Return 4
            Case <= 9
                Return 7
        End Select
        Return 10
    End Function
    '<Extension>
    'Public Function GetQuarter(ByVal dt As Date) As Integer
    '    Return Convert.ToInt32(Math.Ceiling(dt.Month / 3.0))
    'End Function
    <Extension>
    Public Function FirstDayOfQuarter(ByVal dt As Date) As Date
        Return New Date(dt.Year, dt.GetFirstMonthOfQuarter(), 1)
    End Function

    <Extension>
    Public Function LastDayOfQuarter(ByVal dt As Date) As Date
        Return New Date(dt.Year, dt.GetFirstMonthOfQuarter(), 1).AddMonths(3).AddDays(-1)
    End Function

    <Extension>
    Public Function GetWeekNumberOfMonth(ByVal dt As Date) As Integer
        dt = dt.Date
        Dim firstMonthDay As New Date(dt.Year, dt.Month, 1)
        Dim firstMonthMonday As Date = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) Mod 7)
        If firstMonthMonday > dt Then
            firstMonthDay = firstMonthDay.AddMonths(-1)
            firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) Mod 7)
        End If
        Return (dt.Subtract(firstMonthMonday)).Days \ 7 + 1
    End Function


#End Region

    <Extension>
    Public Function IsMathOperation(bo As BinaryOperator) As Boolean
        Select Case bo.OperatorType
            Case BinaryOperatorType.Divide, BinaryOperatorType.Minus, BinaryOperatorType.Modulo, BinaryOperatorType.Multiply, BinaryOperatorType.Plus
                Return True
        End Select
        Return False
    End Function

    <Extension>
    Public Function IsEqualityOperation(bo As BinaryOperator) As Boolean
        Select Case bo.OperatorType
            Case BinaryOperatorType.Equal, BinaryOperatorType.NotEqual, BinaryOperatorType.Greater, BinaryOperatorType.GreaterOrEqual, BinaryOperatorType.Less, BinaryOperatorType.LessOrEqual
                Return True
        End Select
        Return False
    End Function

    <Extension>
    Public Function GetCritriaKind(co As CriteriaOperator) As CriteriaKind
        If TypeOf co Is AggregateOperand Then
            Return CriteriaKind.AggregateOperand
        ElseIf TypeOf co Is BetweenOperator Then
            Return CriteriaKind.BetweenOperator
        ElseIf TypeOf co Is BinaryOperator Then
            Return CriteriaKind.BinaryOperator
        ElseIf TypeOf co Is FunctionOperator Then
            Return CriteriaKind.FunctionOperator
        ElseIf TypeOf co Is GroupOperator Then
            Return CriteriaKind.GroupOperator
        ElseIf TypeOf co Is InOperator Then
            Return CriteriaKind.InOperator
        ElseIf TypeOf co Is JoinOperand Then
            Return CriteriaKind.JoinOperand
        ElseIf TypeOf co Is OperandProperty Then
            Return CriteriaKind.OperandProperty
        ElseIf TypeOf co Is UnaryOperator Then
            Return CriteriaKind.UnaryOperator
        ElseIf TypeOf co Is OperandParameter Then
            Return CriteriaKind.OperandParameter
        ElseIf TypeOf co Is OperandValue Then
            If TypeOf co Is OperandParameter Then
                Return CriteriaKind.OperandParameter
            End If
            Return CriteriaKind.OperandValue
        Else
            Return CriteriaKind.Unknow
        End If

    End Function



#Region "Base"

    <Extension>
    Public Function GetUniqueCombinations(ByVal list As IEnumerable(Of Integer)) As IEnumerable(Of IEnumerable(Of Integer))
        Dim qry = From m In Enumerable.Range(0, 1 << list.Count)
                  Distinct Select String.Join("-",
                   (From i In Enumerable.Range(0, list.Count)
                    Where (m And (1 << i)) <> 0
                    Select q = list(i), s = list(i).ToString()
                    Order By q).Select(Function(q) q.s)) Distinct

        Return qry.Select(Function(q) q.Split("-"c).Select(Function(i) Integer.Parse(i)))
    End Function

    <Extension>
    Public Function IsNumeric(ByVal s As String, Optional trim As Boolean = True) As Boolean
        If trim Then
            s = s.Trim
        End If
        Dim seps As Char() = {"."c, ","c, " "c}
        For Each c As Char In s
            If Not Char.IsDigit(c) AndAlso Not seps.Contains(c) Then
                Return False
            End If
        Next c

        Return True
    End Function



    <Extension>
    Public Function IsCastableTo(ByVal fromType As Type, ByVal toType As Type) As Boolean
        If toType.IsAssignableFrom(fromType) Then Return True
        Dim methods = fromType.GetMethods(BindingFlags.Public Or BindingFlags.Static).Where(Function(m) m.ReturnType Is toType AndAlso (m.Name = "op_Implicit" OrElse m.Name = "op_Explicit"))
        Return methods.Count() > 0
    End Function

    <Extension>
    Public Function IsCastableTo(Of T)(ByVal fromType As Type) As Boolean
        Return fromType.IsCastableTo(GetType(T))
    End Function

    <Extension>
    Public Function IsTrue(b As Boolean?) As Boolean
        Return b.HasValue AndAlso b.Value
    End Function

    <Extension>
    Public Function HasBitFlag(Of T As {Structure, IConvertible, IComparable, IFormattable}, T2 As {Structure, IConvertible, IComparable, IFormattable})(ByVal e As T, ByVal other As T2) As Boolean
        Dim eFlag As UInt64 = Convert.ToUInt64(e)
        Dim otherFlag As UInt64 = Convert.ToUInt64(other)
        Return ((eFlag And otherFlag) = otherFlag)
    End Function

    <Extension>
    Public Function GetBitFlag(Of T As {Structure, IConvertible, IComparable, IFormattable}, T2 As {Structure, IConvertible, IComparable, IFormattable})(ByVal e As T, ByVal other As T2) As T2
        Dim eFlag As UInt64 = Convert.ToUInt64(e)
        Dim t2Values As T2() = CType([Enum].GetValues(GetType(T2)), T2())
        Return t2Values.FirstOrDefault(Function(q) eFlag.HasBitFlag(q))
    End Function

    <Extension>
    Public Function GetExponents(Of T As {Structure, IConvertible, IComparable, IFormattable})(ByVal e As T) As Integer
        Dim x As Int64 = Convert.ToInt64(e)

        Return Convert.ToInt32(Math.Log(x) / Math.Log(2))
    End Function
#End Region


#Region "Functions"

    <Extension>
    Public Function UnSafeRegister(fnc As ICustomFunctionOperator, Optional tryRegisterCriteriaOperatorQueryable As Boolean = True) As ICustomFunctionOperator
        CriteriaOperator.RegisterCustomFunction(fnc)
        If tryRegisterCriteriaOperatorQueryable AndAlso TypeOf fnc Is ICustomCriteriaOperatorQueryable Then
            CustomCriteriaManager.RegisterCriterion(DirectCast(fnc, ICustomCriteriaOperatorQueryable))
        End If
        Return fnc
    End Function

    <Extension>
    Public Function SafeRegister(fnc As ICustomFunctionOperator, Optional tryRegisterCriteriaOperatorQueryable As Boolean = True) As ICustomFunctionOperator
        AcurSoft.Data.Filtering.XpoFunctionsHelper.UnRegisterFunction(fnc.Name)
        Return fnc.UnSafeRegister()
    End Function

#End Region

End Module
