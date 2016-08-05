Imports System.Globalization
Imports System.Text
Imports DevExpress.Data.Filtering

Public NotInheritable Class RemoveDiacriticsFunction
    Implements ICustomFunctionOperator
    Public Const FunctionName As String = "RemoveDiacritics"
    Public Shared ReadOnly Property Instance As RemoveDiacriticsFunction


    Public Sub New()
        _Instance = Me
    End Sub


    Public Function Evaluate(ParamArray ByVal operands() As Object) As Object Implements ICustomFunctionOperator.Evaluate
        Dim sb As New StringBuilder()
        If operands.Length <> 1 Then Return ""

        Dim src As String = CType(operands(0), String)
        For Each c As Char In src.Normalize(NormalizationForm.FormKD)
            Select Case CharUnicodeInfo.GetUnicodeCategory(c)
                Case UnicodeCategory.NonSpacingMark, UnicodeCategory.SpacingCombiningMark, UnicodeCategory.EnclosingMark
                    'do nothing
                    Dim xx = c
                Case Else
                    sb.Append(c)
            End Select
        Next c

        Return sb.ToString()
    End Function

    Private ReadOnly Property Name() As String Implements ICustomFunctionOperator.Name
        Get
            Return RemoveDiacriticsFunction.FunctionName
        End Get
    End Property

    Private Function ResultType(ParamArray ByVal operands() As Type) As Type Implements ICustomFunctionOperator.ResultType
        Return GetType(String)
    End Function
End Class