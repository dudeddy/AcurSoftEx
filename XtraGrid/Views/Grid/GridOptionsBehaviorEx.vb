Imports System.ComponentModel
Imports AcurSoft.XtraGrid.Views.Grid.Bookmarks
Imports AcurSoft.XtraGrid.Views.Grid.Extenders
Imports AcurSoft.XtraGrid.Views.Grid.GridFlags
Imports DevExpress.Utils.Serializing
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid

Namespace AcurSoft.XtraGrid.Views.Grid

    Public Class GridOptionsBehaviorEx
        Inherits GridOptionsBehavior
        <Browsable(False)>
        Public ReadOnly Property ViewEx As GridViewEx

        Private _ColumnsSelectionHelper As GridViewColumnsSelectionHelper
        Public ReadOnly Property ColumnsSelectionHelper As GridViewColumnsSelectionHelper
            Get
                If _ColumnsSelectionHelper Is Nothing Then
                    _ColumnsSelectionHelper = New GridViewColumnsSelectionHelper(Me.ViewEx)
                End If
                Return _ColumnsSelectionHelper
            End Get
        End Property

        Private _BookmarksHelper As GridViewBookmarks
        <Browsable(False)>
        Public ReadOnly Property BookmarksHelper As GridViewBookmarks
            Get
                If _BookmarksHelper Is Nothing Then
                    _BookmarksHelper = New GridViewBookmarks(Me.ViewEx)
                End If
                Return _BookmarksHelper
            End Get
        End Property


        'Private _FlagsHelper As GridViewFlags
        '<Browsable(False)>
        'Public ReadOnly Property FlagsHelper As GridViewFlags
        '    Get
        '        If _FlagsHelper Is Nothing Then
        '            _FlagsHelper = New GridViewFlags(Me.ViewEx)
        '        End If
        '        Return _FlagsHelper
        '    End Get
        'End Property




        Private _BookMarksKeyFieldName As String
        <XtraSerializableProperty, DefaultValue(GetType(String), Nothing)>
        Public Property BookMarksKeyFieldName As String
            Get
                If String.IsNullOrEmpty(_BookMarksKeyFieldName) Then
                    _BookMarksKeyFieldName = Me.BookmarksHelper.KeyFieldName
                End If
                Return _BookMarksKeyFieldName
            End Get
            Set(value As String)
                If _BookMarksKeyFieldName <> value Then
                    _BookMarksKeyFieldName = value
                    If String.IsNullOrEmpty(value) Then
                        _BookmarksHelper = Nothing
                    Else
                        Me.BookmarksHelper.KeyFieldName = value
                    End If
                End If
            End Set
        End Property

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(columnView As ColumnView)
            MyBase.New(columnView)
        End Sub


        Public Sub New(view As GridViewEx)
            MyBase.New(view)
            Me.ViewEx = view
        End Sub

    End Class
End Namespace
