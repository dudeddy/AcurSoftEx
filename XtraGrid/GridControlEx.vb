Imports System.ComponentModel
Imports AcurSoft.XtraGrid.Registrator
Imports AcurSoft.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Base
Namespace AcurSoft.XtraGrid

    <ToolboxItem(True)>
    Public Class GridControlEx
        Inherits GridControl

        Protected Overrides Function CreateDefaultView() As BaseView
            Return CreateView(GridViewEx.VIEW_NAME)
        End Function

        Protected Overrides Sub RegisterAvailableViewsCore(ByVal collection As InfoCollection)
            MyBase.RegisterAvailableViewsCore(collection)
            collection.Add(New GridViewExInfoRegistrator())
        End Sub

    End Class
End Namespace

