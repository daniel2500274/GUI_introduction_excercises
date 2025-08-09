#!/bin/bash

# Define el nombre de la aplicación
APP_NAME="GUI_introduction_excercises"
# Define el directorio de publicación
PUBLISH_DIR="bin/Release/net9.0/win-x64/publish"
# Define el nombre del archivo ZIP
ZIP_NAME="${APP_NAME}_win-x64.zip"
# Define el directorio de producción
PRODUCTION_DIR="produccion"

# Crear el directorio de producción si no existe
mkdir -p "$PRODUCTION_DIR"

# Ruta completa al directorio de producción
FULL_PRODUCTION_DIR="$(pwd)/$PRODUCTION_DIR"

# Verificar si zip está instalado
if ! command -v zip &> /dev/null; then
    echo "El comando zip no está instalado. Por favor, instálalo antes de ejecutar este script."
    exit 1
fi

# Paso 1: Compilar la aplicación para Windows
echo "Compilando la aplicación..."
dotnet publish -c Release -r win-x64 --self-contained true

# Verifica si la compilación fue exitosa
if [ $? -ne 0 ]; then
    echo "Error durante la compilación."
    exit 1
fi

# Verificar si el directorio de publicación existe
if [ ! -d "$PUBLISH_DIR" ]; then
    echo "El directorio de publicación $PUBLISH_DIR no existe."
    exit 1
fi

# Paso 2: Crear el archivo README
echo "Creando el archivo README..."
README_CONTENT="Instrucciones para ejecutar la aplicación ${APP_NAME} en Windows:

1. Descomprime el archivo ZIP en una carpeta de tu elección.
2. Navega a la carpeta donde descomprimiste los archivos.
3. Ejecuta el archivo ${APP_NAME}.exe para iniciar la aplicación.

Requisitos:
- Sistema operativo: Windows 10 o superior.

Nota: Asegúrate de tener permisos para ejecutar el archivo .exe."

# Asegurarse de que el directorio de publicación existe antes de escribir el README
mkdir -p "$PUBLISH_DIR"
echo "$README_CONTENT" > "$PUBLISH_DIR/README.txt"

# Paso 3: Empaquetar todo en un archivo ZIP
echo "Empaquetando los archivos en un ZIP..."
# Usamos la ruta absoluta para evitar problemas con rutas relativas
cd "$(dirname "$(pwd)/$PUBLISH_DIR")" || { echo "No se pudo acceder al directorio padre de $PUBLISH_DIR"; exit 1; }
zip -r "$FULL_PRODUCTION_DIR/$ZIP_NAME" "$(basename "$PUBLISH_DIR")"

echo "Proceso completado. El archivo $ZIP_NAME ha sido creado en el directorio $PRODUCTION_DIR."
