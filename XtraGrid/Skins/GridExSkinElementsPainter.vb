Imports AcurSoft.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Skins
Imports DevExpress.XtraGrid.Views.Base

Namespace AcurSoft.XtraGrid.Skins
    Public Class GridExSkinElementsPainter
        Inherits GridSkinElementsPainter

        Public Sub New(ByVal view As BaseView)
            MyBase.New(view)
        End Sub
        Protected Overrides Function CreateFilterPanelPainter() As GridFilterPanelPainter
            Dim painter As GridFilterPanelPainter = New GridExFilterPanelPainter(View)
            Return painter
        End Function
    End Class
End Namespace
