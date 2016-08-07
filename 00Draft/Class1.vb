Imports System.ComponentModel
Imports DevExpress.Data
Imports DevExpress.Data.Helpers

Public Class Class1
    Public Shared Function Topx(dc As BaseListSourceDataController, fieldName As String, top As Integer) As Object
        'Dim dc As BaseGridController
        Dim h As BaseDataControllerHelper = dc.Helper
        Dim pd As PropertyDescriptor = h.DescriptorCollection.Find(fieldName, True)

        Dim index As Integer = h.DescriptorCollection.IndexOf(pd)
        Dim cnt As Integer = h.List.Count
        Dim data As IEnumerable(Of Object) = Nothing
        If pd.PropertyType Is GetType(TimeSpan) Then
            data = From row In Enumerable.Range(0, cnt) Select q = h.GetRowValue(row, index) Order By q Ascending Take top
            Return TimeSpan.FromSeconds(data.Sum(Function(s) DirectCast(s, TimeSpan).TotalSeconds))
        Else
            data = From row In Enumerable.Range(0, cnt) Select q = h.GetRowValue(row, index) Order By q Ascending Take top
            Return data.Sum(Function(s) Convert.ToDecimal(s))
        End If

    End Function

End Class
