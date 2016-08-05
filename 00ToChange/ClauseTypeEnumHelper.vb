Imports DevExpress.Data.Filtering.Helpers

Public Module ClauseTypeEnumHelper
    Private ReadOnly BaseValue As Integer = Convert.ToInt32(System.Enum.GetValues(GetType(ClauseType)).Cast(Of ClauseType)().Max())

    Public ReadOnly Property MatchesAnyOf() As Integer
        Get
            Return BaseValue + 1
        End Get
    End Property

    Public Function GetMenuStringByClauseType(ByVal clauseType As Integer) As String
        If clauseType = MatchesAnyOf Then
            Return "Matches any of"
        End If
        Return String.Empty
    End Function
End Module
