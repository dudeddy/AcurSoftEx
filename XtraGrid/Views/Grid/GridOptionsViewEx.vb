Imports System.ComponentModel
Imports DevExpress.Utils.Controls
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Namespace AcurSoft.XtraGrid.Views.Grid
    Public Class GridOptionsViewEx
        Inherits GridOptionsView

        <Browsable(False)>
        Public ReadOnly Property ViewEx As GridViewEx

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(view As GridViewEx)
            MyBase.New()
            Me.ViewEx = view
            _EnableSortDialog = True
            _ShowSortIndexInColumnHeader = True
        End Sub

#Region "XtraPanel"

        Private _ShowXtraPanel As Boolean
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(False)>
        Public Property ShowXtraPanel As Boolean
            Get
                Return _ShowXtraPanel
            End Get
            Set(value As Boolean)
                If value = _ShowXtraPanel Then Return
                Dim prevValue As Boolean = _ShowXtraPanel
                If Me.ViewEx.XtraPanel IsNot Nothing Then
                    Me.ViewEx.XtraPanel.Visible = value
                    Me.ViewEx.SplitterControl.Visible = value
                    Me.ViewEx.LayoutChanged()
                End If
                _ShowXtraPanel = value
                OnChanged(New BaseOptionChangedEventArgs("_ShowXtraPanel", prevValue, value))
            End Set
        End Property

        Private _XtraPanelMinimumHight As Integer = 100
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(100)>
        Public Property XtraPanelMinimumHight As Integer
            Get
                Return _XtraPanelMinimumHight
            End Get
            Set(value As Integer)
                If value = _XtraPanelMinimumHight Then Return
                Dim prevValue As Integer = _XtraPanelMinimumHight
                If _ViewEx.XtraPanel IsNot Nothing Then
                    _ViewEx.XtraPanel.MinimumSize = New Size(0, value)
                    _ViewEx.LayoutChanged()
                    'Me.ViewEx.VisualClientUpdateLayout()
                End If
                _XtraPanelMinimumHight = value
                OnChanged(New BaseOptionChangedEventArgs("XtraPanelMinimumHight", prevValue, value))
            End Set
        End Property

        Private _XtraPanelMaximumHight As Integer = 300
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(300)>
        Public Property XtraPanelMaximumHight As Integer
            Get
                Return _XtraPanelMaximumHight
            End Get
            Set(value As Integer)
                If value = _XtraPanelMinimumHight Then Return
                Dim prevValue As Integer = _XtraPanelMaximumHight

                If _ViewEx.XtraPanel IsNot Nothing Then
                    _ViewEx.XtraPanel.MaximumSize = New Size(0, value)
                    _ViewEx.LayoutChanged()

                    'Me.ViewEx.VisualClientUpdateLayout()
                End If
                OnChanged(New BaseOptionChangedEventArgs("XtraPanelMaximumHight", prevValue, value))

                _XtraPanelMaximumHight = value

            End Set
        End Property
#End Region

        Private _FillEmptySpace As Boolean
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(False)>
        Public Property FillEmptySpace() As Boolean
            Get
                Return _FillEmptySpace
            End Get
            Set(value As Boolean)
                If value = _FillEmptySpace Then Return
                Dim prevValue As Boolean = _FillEmptySpace
                If value Then
                    If Me.ColumnAutoWidth Then
                        Me.ColumnAutoWidth = False
                    End If
                End If
                _FillEmptySpace = value
                OnChanged(New BaseOptionChangedEventArgs("FillEmptySpace", prevValue, value))
            End Set
        End Property

        Private _EnableSortDialog As Boolean
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(True)>
        Public Property EnableSortDialog As Boolean
            Get
                Return _EnableSortDialog
            End Get
            Set(value As Boolean)
                If value = _EnableSortDialog Then Return
                Dim prevValue As Boolean = _EnableSortDialog
                'If value Then
                '    If Me.ColumnAutoWidth Then
                '        Me.ColumnAutoWidth = False
                '    End If
                'End If
                _EnableSortDialog = value
                OnChanged(New BaseOptionChangedEventArgs("EnableSortDialog", prevValue, value))

            End Set
        End Property


        Private _ShowSortIndexInColumnHeader As Boolean
        <XtraSerializableProperty, XtraSerializablePropertyId(2), DefaultValue(True)>
        Public Property ShowSortIndexInColumnHeader() As Boolean
            Get
                Return _ShowSortIndexInColumnHeader
            End Get
            Set(ByVal value As Boolean)
                If value = _ShowSortIndexInColumnHeader Then Return
                Dim prevValue As Boolean = _ShowSortIndexInColumnHeader

                _ShowSortIndexInColumnHeader = value
                For Each col As GridColumn In Me.ViewEx.SortedColumns
                    Me.ViewEx.InvalidateColumnHeader(col)
                Next col
                OnChanged(New BaseOptionChangedEventArgs("ShowSortIndexInColumnHeader", prevValue, value))
            End Set
        End Property
    End Class
End Namespace
