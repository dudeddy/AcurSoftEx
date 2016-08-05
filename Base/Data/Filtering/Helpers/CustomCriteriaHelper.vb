Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Data.Helpers
Imports DevExpress.Data.Filtering
Imports System.Threading
Imports DevExpress.Data.Summary

Namespace AcurSoft.Data.Filtering.Helpers


    Public Class CustomCriteriaHelper
        Public Shared Function Create(ByVal parseResult As FindSearchParserResults, ByVal defaultCondition As FilterCondition, ByVal isServerMode As Boolean) As CriteriaOperator
            Dim rv As CriteriaOperator = Nothing
            rv = CreateFilter(parseResult.SearchTexts, parseResult.ColumnNames, defaultCondition, isServerMode)
            For Each f As FindSearchField In parseResult.Fields
                Dim columnFilter As CriteriaOperator = Nothing
                columnFilter = CreateFilter(f.Values, New FindColumnInfo() {f.Column}, defaultCondition, isServerMode)
                rv = rv And columnFilter
            Next f
            Return rv
        End Function
        Private Shared Function CreateFilter(ByVal values() As String, ByVal properties() As FindColumnInfo, ByVal filterCondition As FilterCondition, ByVal isServerMode As Boolean) As CriteriaOperator
            Dim stAnd As CriteriaOperator = Nothing, stOr As CriteriaOperator = Nothing
            For Each stext As String In values
                If stext.StartsWith("+") Then
                    stAnd = stAnd And DoFilterCondition(stext.Substring(1), properties, filterCondition, isServerMode)
                ElseIf (stext.StartsWith("-"))
                    stAnd = stAnd And Not DoFilterCondition(stext.Substring(1), properties, filterCondition, isServerMode)
                End If
                stOr = stOr Or DoFilterCondition(stext, properties, filterCondition, isServerMode)
            Next stext
            Return stAnd And stOr
        End Function
        Private Shared Function DoFilterCondition(ByVal originalValue As String, ByVal columns() As FindColumnInfo, ByVal defaultCondition As FilterCondition, ByVal isServerMode As Boolean) As CriteriaOperator
            Dim rv As CriteriaOperator = Nothing
            Dim op As CriteriaOperator
            For Each column As FindColumnInfo In columns
                Dim filterCondition As FilterCondition = defaultCondition
                Dim value As Object = originalValue
                If isServerMode Then
                    If Not AllowColumn(column, value, filterCondition) Then
                        Continue For
                    End If
                End If
                Dim [property] As New OperandProperty(column.PropertyName)
                Select Case filterCondition
                    Case FilterCondition.StartsWith
                        op = New FunctionOperator(FunctionOperatorType.StartsWith, [property], New OperandValue(value))
                    Case FilterCondition.Contains
                        op = New FunctionOperator(FunctionOperatorType.Contains, [property], New OperandValue(value))
                    Case FilterCondition.Equals
                        op = FilterHelper.CalcColumnFilterCriteriaByValue(column.PropertyName, column.Column.FieldType, value, True, Thread.CurrentThread.CurrentCulture)
                    Case Else
                        'op = If(value.ToString().Contains("%"), CType(New BinaryOperator([property], New OperandValue(value), BinaryOperatorType.Like), CriteriaOperator), New FunctionOperator(FunctionOperatorType.Contains, [property], New OperandValue(value)))
                        op = New FunctionOperator(FunctionOperatorType.Contains, [property], New OperandValue(value))
                End Select
                rv = rv Or op
            Next column
            Return rv
        End Function
        Private Shared Function AllowColumn(ByVal column As FindColumnInfo, ByRef value As Object, ByRef filterCondition As FilterCondition) As Boolean
            If column.Column Is Nothing Then
                Return False
            End If
            Dim type As Type = column.Column.FieldType
            If Nullable.GetUnderlyingType(type) IsNot Nothing Then
                type = Nullable.GetUnderlyingType(type)
            End If
            Dim val As String = If(value Is Nothing, Nothing, value.ToString())
            If SummaryItemTypeHelper.IsNumericalType(type) Then
                filterCondition = FilterCondition.Equals
                Dim numVal As Object
                Try
#If Not SL Then
                    numVal = Convert.ChangeType(val, type)
#Else
                    numVal = Convert.ChangeType(val, type, CultureInfo.CurrentCulture)
#End If
                Catch
                    Return False
                End Try
                value = numVal
                Return True
            End If
            If SummaryItemTypeHelper.IsDateTime(type) Then
                filterCondition = FilterCondition.Equals
                Dim [date] As Object
                Try
#If Not SL Then
                    [date] = Convert.ChangeType(val, type)
#Else
                    [date] = Convert.ChangeType(val, type, CultureInfo.CurrentCulture)
#End If
                Catch
                    Return False
                End Try
                value = [date]
                Return True
            End If
            If SummaryItemTypeHelper.IsBool(type) Then
                filterCondition = FilterCondition.Equals
                Dim res As Boolean = Nothing
                If Not Boolean.TryParse(val, res) Then
                    Return False
                End If
                value = res
                Return True
            End If
#If SL Then
            If type.Equals(GetType(TimeSpan)) OrElse type.Equals(GetType(TimeSpan?)) Then
                filterCondition = FilterCondition.Equals
                Dim res As TimeSpan = Nothing
                If Not TimeSpan.TryParse(val, res) Then
                    Return False
                End If
                value = res
                Return True
            End If
#End If
            Return True
        End Function
        Public NotInheritable Class FilterCriteriaHelper

            Private Sub New()
            End Sub

            Public Shared Function ReplaceFilterCriteria(ByVal source As CriteriaOperator, ByVal prevOperand As CriteriaOperator, ByVal newOperand As CriteriaOperator) As CriteriaOperator
                Dim groupOperand As GroupOperator = TryCast(source, GroupOperator)
                If ReferenceEquals(groupOperand, Nothing) Then
                    Return newOperand
                End If
                Dim clone As GroupOperator = groupOperand.Clone()
                clone.Operands.Remove(prevOperand)
                If clone.Equals(source) Then
                    Return newOperand
                End If
                clone.Operands.Add(newOperand)
                Return clone
            End Function
        End Class
    End Class

End Namespace
