Imports System.Reflection
Imports AcurSoft.Data.Filtering
Imports AcurSoft.XtraEditors.Repository
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.XtraEditors.Repository

Public Class XpoFunctionFilterManager
    Private Shared _ClauseTypeBaseValue As Integer = 0
    Public Shared ReadOnly Property ClauseTypeBaseValue As Integer
        Get
            If _ClauseTypeBaseValue = 0 Then
                _ClauseTypeBaseValue = Convert.ToInt32(System.Enum.GetValues(GetType(ClauseType)).Cast(Of ClauseType)().Max()) + 10
            End If
            Return _ClauseTypeBaseValue
        End Get
    End Property


    Private Shared _FunctionsFilterInfos As Dictionary(Of String, XpoFunctionFilterInfo)
    Public Shared ReadOnly Property FunctionsFilterInfos As Dictionary(Of String, XpoFunctionFilterInfo)
        Get
            If _FunctionsFilterInfos Is Nothing Then
                _FunctionsFilterInfos = New Dictionary(Of String, XpoFunctionFilterInfo)
            End If
            Return _FunctionsFilterInfos
        End Get
    End Property

    Public Shared Function GetEditorByType(type As Type) As RepositoryItem
        If type Is Nothing Then
            Return New RepositoryItemTextEdit
        ElseIf type Is GetType(Integer)
            Return New RepositoryItemSpinEdit With {.IsFloatValue = False}
        ElseIf type.IsCastableTo(GetType(Decimal))
            Return New RepositoryItemSpinEdit
        ElseIf type.IsCastableTo(GetType(Date))
            Return New RepositoryItemDateEdit

        End If
        Return New RepositoryItemTextEdit
    End Function


    Public Shared Function Add(xpoFunctionName As String) As XpoFunctionFilterInfo
        If XpoFunctionFilterManager.HasInfos(xpoFunctionName) Then Return Nothing
        Dim iCustomFunctionOperator As ICustomFunctionOperator = CriteriaOperator.GetCustomFunction(xpoFunctionName)
        If TypeOf iCustomFunctionOperator IsNot XpoFunctionBase Then Return Nothing
        Dim xpoFunctionFilterInfo As New XpoFunctionFilterInfo(DirectCast(iCustomFunctionOperator, XpoFunctionBase))
        xpoFunctionFilterInfo.Id = XpoFunctionFilterManager.ClauseTypeBaseValue
        _ClauseTypeBaseValue += 1
        XpoFunctionFilterManager.FunctionsFilterInfos.Add(xpoFunctionName, xpoFunctionFilterInfo)
        Return xpoFunctionFilterInfo
    End Function

    Public Shared Function HasInfos(xpoFunctionName As String) As Boolean
        Return XpoFunctionFilterManager.FunctionsFilterInfos.ContainsKey(xpoFunctionName)
    End Function
    Public Shared Function GetInfos(xpoFunctionName As String) As XpoFunctionFilterInfo
        If Not XpoFunctionFilterManager.HasInfos(xpoFunctionName) Then Return Nothing
        Return XpoFunctionFilterManager.FunctionsFilterInfos(xpoFunctionName)
    End Function

    Public Shared Function GetInfos(id As Integer) As XpoFunctionFilterInfo
        Return XpoFunctionFilterManager.FunctionsFilterInfos.Values.FirstOrDefault(Function(q) q.Id = id)
    End Function

    Public Shared Sub Init()
        Dim yearValueFixer As Func(Of Object, Object) =
            Function(q)
                Dim defaultValue As Integer = Today.Year
                Try
                    Dim year As Integer = Convert.ToInt32(q)
                    If year < 1 OrElse year > 4000 Then
                        Return defaultValue
                    End If
                    Return year
                Catch ex As Exception
                    Return defaultValue
                End Try
            End Function
        Dim monthValueFixer As Func(Of Object, Object) =
            Function(q)
                Dim defaultValue As Integer = Today.Month
                Try
                    Dim month As Integer = Convert.ToInt32(q)
                    If month < 1 OrElse month > 12 Then
                        Return defaultValue
                    End If
                    Return month
                Catch ex As Exception
                    Return defaultValue
                End Try
            End Function
        Dim quarterValueFixer As Func(Of Object, Object) =
            Function(q)
                Dim defaultValue As Integer = Today.GetQuarter
                Try
                    Dim quarter As Integer = Convert.ToInt32(q)
                    If quarter < 1 OrElse quarter > 4 Then
                        Return defaultValue
                    End If
                    Return quarter
                Catch ex As Exception
                    Return defaultValue
                End Try
            End Function

        Dim dayValueFixer As Func(Of Object, Object) =
            Function(q)
                Dim defaultValue As Integer = 1
                Try
                    Dim day As Integer = Convert.ToInt32(q)
                    If day < 1 OrElse day > 31 Then
                        Return defaultValue
                    End If
                    Return day
                Catch ex As Exception
                    Return defaultValue
                End Try
            End Function
        'InYear
        Dim InYear As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("InYear")
        With InYear.Parameters(1)
            .EditorCreator = Function() New RepositoryItemYearEdit
            .ValueFixer = yearValueFixer
        End With

        Dim InYearMonth As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("InYearMonth")
        With InYearMonth.Parameters(1)
            .EditorCreator = Function() New RepositoryItemYearEdit ' With {.SpinButtonIndex = 0}
            .ValueFixer = yearValueFixer
        End With
        With InYearMonth.Parameters(2)
            .EditorCreator = Function() New RepositoryItemMonthEditEx ' With {.SpinButtonIndex = 0}
            .ValueFixer = monthValueFixer
        End With

        Dim InYearQuarter As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("InYearQuarter")
        With InYearQuarter.Parameters(1)
            .EditorCreator = Function() New RepositoryItemYearEdit ' With {.SpinButtonIndex = 0}
            .ValueFixer = yearValueFixer
        End With
        With InYearQuarter.Parameters(2)
            .EditorCreator = Function() New RepositoryItemQuarterEdit ' With {.SpinButtonIndex = 0}
            .ValueFixer = quarterValueFixer
        End With

        Dim GetDate As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("XGetDate")
        With GetDate.Parameters(0)
            .EditorCreator = Function() New RepositoryItemYearEdit ' With {.SpinButtonIndex = 0}
            .ValueFixer = yearValueFixer
        End With
        With GetDate.Parameters(1)
            .EditorCreator = Function() New RepositoryItemMonthEditEx ' With {.SpinButtonIndex = 0}
            .ValueFixer = monthValueFixer
        End With
        With GetDate.Parameters(2)
            .EditorCreator = Function() New RepositoryItemDayOfMonthEditEx
            .ValueFixer = dayValueFixer
        End With

        Dim GetMonth As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("XGetMonth")
        Dim GetYear As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("XGetYear")
        Dim XToday As XpoFunctionFilterInfo = XpoFunctionFilterManager.Add("XToday")
    End Sub
End Class



