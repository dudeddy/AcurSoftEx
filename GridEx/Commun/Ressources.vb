Imports System.IO

Public Class Ressources
    Private Shared _Assembly As Reflection.Assembly

    Shared Sub New()
        _Assembly = GetType(Ressources).Assembly
    End Sub
    Private Shared _bookmark_filter_select_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_filter_select_16x16 As Bitmap
        Get
            If _bookmark_filter_select_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-filter-select_16x16.png")
                    _bookmark_filter_select_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_filter_select_16x16
        End Get
    End Property
    Private Shared _bookmark_filter_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_filter_16x16 As Bitmap
        Get
            If _bookmark_filter_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-filter_16x16.png")
                    _bookmark_filter_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_filter_16x16
        End Get
    End Property

    Private Shared _bookmark_filter_deabled_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_filter_deabled_16x16 As Bitmap
        Get
            If _bookmark_filter_deabled_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-filter-deabled_16x16.png")
                    _bookmark_filter_deabled_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_filter_deabled_16x16
        End Get
    End Property

    Private Shared _bookmark_delete_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_delete_16x16 As Bitmap
        Get
            If _bookmark_delete_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-delete_16x16.png")
                    _bookmark_delete_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_delete_16x16
        End Get
    End Property

    Private Shared _bookmark_add_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_add_16x16 As Bitmap
        Get
            If _bookmark_add_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-add_16x16.png")
                    _bookmark_add_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_add_16x16
        End Get
    End Property

    Private Shared _bookmark_remove_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_remove_16x16 As Bitmap
        Get
            If _bookmark_remove_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark-remove_16x16.png")
                    _bookmark_remove_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_remove_16x16
        End Get
    End Property

    Private Shared _bookmark_16x16 As Bitmap
    Public Shared ReadOnly Property bookmark_16x16 As Bitmap
        Get
            If _bookmark_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("bookmark_16x16.png")
                    _bookmark_16x16 = New Bitmap(s)
                End Using
            End If
            Return _bookmark_16x16
        End Get
    End Property

    Private Shared _functions_16x16 As Bitmap
    Public Shared ReadOnly Property functions_16x16 As Bitmap
        Get
            If _functions_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("functions_16x16.png")
                    _functions_16x16 = New Bitmap(s)
                End Using
            End If
            Return _functions_16x16
        End Get
    End Property

    Private Shared _date_16x16 As Bitmap
    Public Shared ReadOnly Property date_16x16 As Bitmap
        Get
            If _date_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("date_16x16.png")
                    _date_16x16 = New Bitmap(s)
                End Using
            End If
            Return _date_16x16
        End Get
    End Property

    Private Shared _criteria_operator_between_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_between_16x16 As Bitmap
        Get
            If _criteria_operator_between_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_between_16x16.png")
                    _criteria_operator_between_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_between_16x16
        End Get
    End Property


    Private Shared _cog_edit_16x16 As Bitmap
    Public Shared ReadOnly Property cog_edit_16x16 As Bitmap
        Get
            If _cog_edit_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("cog_edit_16x16.png")
                    _cog_edit_16x16 = New Bitmap(s)
                End Using
            End If
            Return _cog_edit_16x16
        End Get
    End Property

    Private Shared _criteria_operator_ne_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_ne_16x16 As Bitmap
        Get
            If _criteria_operator_ne_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_ne_16x16.png")
                    _criteria_operator_ne_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_ne_16x16
        End Get
    End Property

    Private Shared _criteria_operator_lt_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_lt_16x16 As Bitmap
        Get
            If _criteria_operator_lt_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_lt_16x16.png")
                    _criteria_operator_lt_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_lt_16x16
        End Get
    End Property

    Private Shared _criteria_operator_le_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_le_16x16 As Bitmap
        Get
            If _criteria_operator_le_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_le_16x16.png")
                    _criteria_operator_le_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_le_16x16
        End Get
    End Property

    Private Shared _criteria_operator_gt_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_gt_16x16 As Bitmap
        Get
            If _criteria_operator_gt_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_gt_16x16.png")
                    _criteria_operator_gt_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_gt_16x16
        End Get
    End Property

    Private Shared _criteria_operator_ge_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_ge_16x16 As Bitmap
        Get
            If _criteria_operator_ge_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_ge_16x16.png")
                    _criteria_operator_ge_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_ge_16x16
        End Get
    End Property

    Private Shared _criteria_operator_equal_16x16 As Bitmap
    Public Shared ReadOnly Property criteria_operator_equal_16x16 As Bitmap
        Get
            If _criteria_operator_equal_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("criteria_operator_equal_16x16.png")
                    _criteria_operator_equal_16x16 = New Bitmap(s)
                End Using
            End If
            Return _criteria_operator_equal_16x16
        End Get
    End Property


    Private Shared _table_field_16x16 As Bitmap
    Public Shared ReadOnly Property table_field_16x16 As Bitmap
        Get
            If _table_field_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("table_field_16x16.png")
                    _table_field_16x16 = New Bitmap(s)
                End Using
            End If
            Return _table_field_16x16
        End Get
    End Property


    Private Shared _filter_visual_16x16 As Bitmap
    Public Shared ReadOnly Property filter_visual_16x16 As Bitmap
        Get
            If _filter_visual_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("filter_visual_16x16.png")
                    _filter_visual_16x16 = New Bitmap(s)
                End Using
            End If
            Return _filter_visual_16x16
        End Get
    End Property

    Private Shared _filter_text_16x16 As Bitmap
    Public Shared ReadOnly Property filter_text_16x16 As Bitmap
        Get
            If _filter_text_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("filter_text_16x16.png")
                    _filter_text_16x16 = New Bitmap(s)
                End Using
            End If
            Return _filter_text_16x16
        End Get
    End Property

    Private Shared _filter_16x16 As Bitmap
    Public Shared ReadOnly Property filter_16x16 As Bitmap
        Get
            If _filter_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("filter_16x16.png")
                    _filter_16x16 = New Bitmap(s)
                End Using
            End If
            Return _filter_16x16
        End Get
    End Property

    Private Shared _filter_delete_16x16 As Bitmap
    Public Shared ReadOnly Property filter_delete_16x16 As Bitmap
        Get
            If _filter_delete_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("filter_delete_16x16.png")
                    _filter_delete_16x16 = New Bitmap(s)
                End Using
            End If
            Return _filter_delete_16x16
        End Get
    End Property

    Private Shared _settings_options_16x16 As Bitmap
    Public Shared ReadOnly Property settings_options_16x16 As Bitmap
        Get
            If _settings_options_16x16 Is Nothing Then
                Using s As Stream = _Assembly.GetManifestResourceStream("settings_options_16x16.png")
                    _settings_options_16x16 = New Bitmap(s)
                End Using
            End If
            Return _settings_options_16x16
        End Get
    End Property
End Class
