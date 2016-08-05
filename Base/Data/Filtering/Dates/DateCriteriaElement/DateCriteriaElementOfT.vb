
Imports DevExpress.Data.Filtering


Namespace AcurSoft.Data.Filtering


    Public MustInherit Class DateCriteriaElement(Of T)
        Inherits DateCriteriaElementBase(Of T)
        Public Overridable Property ThisCriteria As String
        Public Property DateChooseKind As DateChooseKind = DateChooseKind.Choose
        Public Overridable Property AfterAgoValue As T

        Public MustOverride Function GetAfterAgoValueCriteria() As CriteriaOperator





        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(v As T)
            MyBase.New(v)
        End Sub

        Public Sub New(v As T, thisValue As T)
            MyBase.New(v)
            If thisValue.Equals(v) Then
                _DateChooseKind = DateChooseKind.This
            End If
        End Sub
    End Class


End Namespace
