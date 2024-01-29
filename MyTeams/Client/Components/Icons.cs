﻿namespace MyTeams.Client.Components;

public enum Icons {
    [GoogleIcon("")] Check,
    [GoogleIcon("")] Error,
    [GoogleIcon("")] Download,
    [GoogleIcon("")] Settings,
    [GoogleIcon("")] Reports,
    [GoogleIcon("")] None,
    [GoogleIcon("")] Add,
    [GoogleIcon("")] Remove,
    Edit,
    check_box,
    check_box_outline_blank,
    list,
    visibility,
    visibility_off,
    [GoogleIcon("")] Reset,
    [GoogleIcon("")] Back,
    [GoogleIcon("")] Forward,
    [GoogleIcon("")] History,
    [GoogleIcon("")] Undo,
    [GoogleIcon("")] Redo,
    [GoogleIcon("")] Clone,
    [GoogleIcon("")] Delete,
    [GoogleIcon("")] NewFolder,
    [GoogleIcon("")] Save,
    [GoogleIcon("")] ChevronDown,
    [GoogleIcon("")] ChevronUp,
    [GoogleIcon("")] ChevronRight,
    [GoogleIcon("")] ChevronLeft,
    [GoogleIcon("")] ChevronRightSmall,
    [GoogleIcon("")] Eye,
    [GoogleIcon("")] Close,
    [GoogleIcon("")] Underline,
    [GoogleIcon("")] Bold,
    [GoogleIcon("")] Heart,
    [GoogleIcon("")] Font,
    [GoogleIcon("")] Copy,
    [GoogleIcon("")] Replay,
    [GoogleIcon("")] ArrowRight,
    [GoogleIcon("")] ArrowLeft,
    [GoogleIcon("")] Active,
    [GoogleIcon("")] swap_horiz,
    [GoogleIcon("")] Star,
    [GoogleIcon("")] Grid,
    [GoogleIcon("")] FolderHorizontal,
    [GoogleIcon("")] OpenedFolderHorizontal,
    [GoogleIcon("")] Info,
    [GoogleIcon("")] Graph,
    [GoogleIcon("")] StatEditor,
    [GoogleIcon("")] HudEditor,
    [GoogleIcon("")] PopupEditor,
    [GoogleIcon("")] MarkedHands,
    [GoogleIcon("")] Statistics,
    [GoogleIcon("")] Sessions,
    [GoogleIcon("")] SwitchUser,
    [GoogleIcon("")] Note,
    [GoogleIcon("")] OpenInNewWindow,
    [GoogleIcon("")] Pin,
    [GoogleIcon("")] PageLeft,
    [GoogleIcon("")] PageRight,
    [GoogleIcon("")] Lock,
    [GoogleIcon("")] Refresh,
    [GoogleIcon("")] Change,
    [GoogleIcon("")] Load,
}

public class GoogleIconAttribute : Attribute {
    public string Value { get; }
    public GoogleIconAttribute(string value) => Value = value;
}
