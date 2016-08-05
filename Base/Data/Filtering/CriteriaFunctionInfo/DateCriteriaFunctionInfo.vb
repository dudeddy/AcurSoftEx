Imports AcurSoft.Data.Filtering
Imports DevExpress.Data.Filtering
Namespace AcurSoft.Data.Filtering

    Public Class DateCriteriaFunctionInfo
        Inherits CriteriaFunctionBaseInfo
        Public Property DateFunction As XpoFunctionForDates
        Public ReadOnly Property Kind As DateFunctionKind
            Get
                If DateFunction Is Nothing Then Return DateFunctionKind.None
                Return DateFunction.Kind
            End Get
        End Property
        Public Sub New()

        End Sub
        Public Sub New(dateFunction As XpoFunctionForDates)
            MyBase.New(dateFunction)
            Me.DateFunction = dateFunction
        End Sub

    End Class
End Namespace
