# Makefile for NyIV
# =================================================================================

CC = gmcs
CFLAGS = 

# ===================================== Packages ===========================
PKG_GTK = -pkg:gtk-sharp-2.0
PKG_GLADE = -pkg:glade-sharp-2.0
PKG_NIRY = -pkg:niry-sharp
PKGS = $(PKG_GTK) $(PKG_GLADE) $(PKG_NIRY)
# ==========================================================================

# ===================================== NyIV Info ======================
NAME = NyIV
VERSION = 0.0.9
COMPILER = `$(CC) --version`
# ==========================================================================

GUI_SRC =	GUI/Dialogs/About.cs			\
			GUI/Dialogs/LoadWebImage.cs		\
			GUI/Dialogs/ProxySettings.cs	\
			GUI/ImageList.cs				\
			GUI/StockIcons.cs				\
			GUI/UiMenuManager.cs			\
			GUI/Viewer.cs					\
			GUI/Window.cs					\
			Info.cs

PLUGIN_LIB_SRC = 	PluginLib/Plugin.cs			\
					PluginLib/PluginManager.cs

UTILS_SRC = Utils/Paths.cs		\
			Utils/Proxy.cs		\
			Utils/WebFetch.cs

SRC = $(GUI_SRC) $(PLUGIN_LIB_SRC) $(UTILS_SRC)

GLADE_RES = Glade/LoadWebImageDialog.glade	\
			Glade/ProxySettingsDialog.glade

IMG_RES = 	Pixmaps/NyIVIcon.png	\
			Pixmaps/NyIVLogo.png	\
			Pixmaps/NyIVImage.png	\
			Pixmaps/NyIVProxy.png

RESOURCES = $(GLADE_RES) $(IMG_RES)

NyIV: NyIVPlugin Main.cs
	@echo
	@echo "$(NAME) Main Application"
	@echo "======================================================="
	$(CC) $(CFLAGS) $(PKGS) Main.cs -out:NyIV.exe -r:nyiv-plugin.dll

NyIVPlugin: NyIVInfo $(SRC) $(RESOURCES)
	@echo
	@echo "$(NAME) Plugin (DLL)"
	@echo "======================================================="
	$(CC) $(CFLAGS) -target:library $(PKGS) $(SRC) $(RESOURCES:%=-resource:%) -out:nyiv-plugin.dll
	@echo "======================================================="
	@echo "gmcs -target:library MyPlugin.cs -r:nyiv-plugin.dll"
	@echo "======================================================="
	@echo

NyIVInfo: Info.cs.in
	@echo
	@echo "Name: $(NAME)"
	@echo "Version: $(VERSION)"
	@echo "Compiler: $(COMPILER)"
	@echo "Install Directory: $(INSTALLDIR)"
	@echo "======================================================="
	sed -e "s|\@version\@|${VERSION}|" 			\
	    -e "s|\@name\@|${NAME}|" 				\
	    -e "s|\@compiler\@|${COMPILER}|" 		\
		 Info.cs.in > Info.cs
	@echo "======================================================="
	@echo
