Imports AcurSoft.XtraGrid.Registrator
Imports AcurSoft.XtraGrid.Views.Grid
Imports AcurSoft.XtraGrid.Views.Grid.Handler
Imports AcurSoft.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Base.Handler
Imports DevExpress.XtraGrid.Views.Base.ViewInfo
Namespace AcurSoft.XtraGrid.Registrator

    Public Class GridViewExInfoRegistrator
        Inherits GridInfoRegistrator

        Public Overrides ReadOnly Property ViewName() As String
            Get
                Return GridViewEx.VIEW_NAME
            End Get
        End Property

        Private _GridExSkinPaintStyle As GridExSkinPaintStyle
        Public Overrides Function PaintStyleByLookAndFeel(ByVal lookAndFeel As DevExpress.LookAndFeel.UserLookAndFeel, ByVal name As String) As ViewPaintStyle
            If _GridExSkinPaintStyle Is Nothing Then
                _GridExSkinPaintStyle = New GridExSkinPaintStyle()
                PaintStyles.Add(_GridExSkinPaintStyle)
            End If
            Return _GridExSkinPaintStyle
        End Function
        Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
            Return New GridViewEx(grid)
        End Function

        Public Overrides Function CreateHandler(ByVal view As BaseView) As BaseViewHandler
            Return New GridViewExHandler(TryCast(view, GridViewEx))
        End Function

        Public Overrides Function CreateViewInfo(ByVal view As BaseView) As BaseViewInfo
            Return New GridViewExInfo(TryCast(view, GridViewEx))
        End Function
    End Class
End Namespace
