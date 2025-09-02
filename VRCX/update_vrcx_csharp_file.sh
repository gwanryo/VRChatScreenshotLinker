#!/bin/bash

TARGET_DIR="."

URLS=(
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/PNGChunk.cs"
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/PNGChunkTypeFilter.cs"
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/PNGFile.cs"
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/PNGHelper.cs"
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/ScreenshotHelper.cs"
    "https://raw.githubusercontent.com/vrcx-team/VRCX/refs/heads/master/Dotnet/ScreenshotMetadata/ScreenshotMetadata.cs"
)

# 각 URL에서 파일을 다운로드
for URL in "${URLS[@]}"; do
    FILENAME=$(basename "$URL")
    OUTPUT_PATH="${TARGET_DIR}/${FILENAME}"
    
    echo "  > 파일 다운로드: ${FILENAME}"
    
    # curl을 사용하여 파일 다운로드 및 덮어쓰기
    curl -o "$OUTPUT_PATH" -s "$URL"
    
    # 다운로드 성공 여부 확인
    if [ $? -eq 0 ]; then
      echo "    - 완료"
    else
      echo "    - 파일 다운로드 실패"
    fi
done
