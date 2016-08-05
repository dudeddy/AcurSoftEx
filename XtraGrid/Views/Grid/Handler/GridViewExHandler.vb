Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.Handler
Namespace AcurSoft.XtraGrid.Views.Grid.Handler

    Public Class GridViewExHandler
        Inherits GridHandler

        Public Sub New(ByVal view As GridView)
            MyBase.New(view)
        End Sub
        Public Shadows ReadOnly Property View() As GridViewEx
            Get
                Return TryCast(MyBase.View, GridViewEx)
            End Get
        End Property
        Protected Overrides Function OnMouseDown(ByVal ev As MouseEventArgs) As Boolean
            View.UpdateFilterPanelButtonState(ObjectState.Pressed, ev.Location)
            Return MyBase.OnMouseDown(ev)
        End Function
        Protected Overrides Function OnMouseMove(ByVal ev As MouseEventArgs) As Boolean
            View.UpdateFilterPanelButtonState(ObjectState.Hot, ev.Location)
            Return MyBase.OnMouseMove(ev)
        End Function
        Protected Overrides Function OnMouseUp(ByVal ev As MouseEventArgs) As Boolean
            View.UpdateFilterPanelButtonState(ObjectState.Normal, ev.Location)
            Return MyBase.OnMouseUp(ev)
        End Function
    End Class
End Namespace
