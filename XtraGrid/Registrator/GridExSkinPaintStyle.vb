Imports AcurSoft.XtraGrid.Skins
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Base
Namespace AcurSoft.XtraGrid.Registrator

    Public Class GridExSkinPaintStyle
        Inherits GridSkinPaintStyle

        Public Sub New()
        End Sub
        Public Overrides Function CreateElementsPainter(ByVal view As BaseView) As DevExpress.XtraGrid.Views.Grid.Drawing.GridElementsPainter
            Return New GridExSkinElementsPainter(view)
        End Function
    End Class
End Namespace

