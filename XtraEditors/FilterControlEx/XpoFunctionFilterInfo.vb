
Imports AcurSoft.Data.Filtering

Imports DevExpress.XtraEditors.Repository

Public Class XpoFunctionFilterInfo
    Inherits CriteriaFunctionBaseInfo


    'Public ReadOnly Property XpoFunction As XpoFunctionBase
    'Public ReadOnly Property CriteriaInfo As CriteriaFunctionBaseInfo

    Private _Parameters As Dictionary(Of Integer, XpoFunctionParamererFilterInfo)
    Public ReadOnly Property Parameters As Dictionary(Of Integer, XpoFunctionParamererFilterInfo)
        Get
            If _Parameters Is Nothing Then
                _Parameters = New Dictionary(Of Integer, XpoFunctionParamererFilterInfo)
            End If
            Return _Parameters
        End Get
    End Property

    Public Function GetFilterParameterInfo(index As Integer) As XpoFunctionParamererFilterInfo
        'Dim index As Integer = Convert.ToInt32((filterIndex / 2) - 1) + 1
        If Not Me.Parameters.ContainsKey(index) Then Return Nothing
        Return Me.Parameters(index)
    End Function

    Public Function GetFilterParameterEditor(index As Integer) As RepositoryItem
        Dim filterParameterInfo As XpoFunctionParamererFilterInfo = Me.GetFilterParameterInfo(index)
        If filterParameterInfo Is Nothing Then Return Nothing
        If filterParameterInfo.EditorCreator Is Nothing Then Return Nothing
        Return filterParameterInfo.EditorCreator.Invoke
    End Function

    Public ReadOnly Property IsConstant() As Boolean
    '    Get
    '        Return Me.Parameters.Count = 0
    '    End Get
    'End Property

    Public ReadOnly Property FirstParamType() As Type
    Public ReadOnly Property ResultType() As Type



    'Public Sub New(criteriaFunctionBaseInfo As CriteriaFunctionBaseInfo)
    '    Me.New(criteriaFunctionBaseInfo.XpoFunction)
    '    'Me.CriteriaInfo = criteriaFunctionBaseInfo
    'End Sub
    Public Sub New(xpoFunction As XpoFunctionBase)
        MyBase.New(xpoFunction)
        _IsConstant = xpoFunction.ArgsCount = 0
        _ResultType = xpoFunction.ResultType
        For i As Integer = 0 To xpoFunction.ArgsCount - 1
            If i = 0 Then
                _FirstParamType = xpoFunction.ArgsInfos(i).ParameterType
            End If
            Me.Parameters.Add(i, New XpoFunctionParamererFilterInfo(xpoFunction, xpoFunction.ArgsInfos(i), i))
        Next
    End Sub

End Class
