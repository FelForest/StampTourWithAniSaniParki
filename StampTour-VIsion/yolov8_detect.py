from collections import defaultdict
import cv2
import numpy as np
import urllib.request

from ultralytics import YOLO


model = YOLO('best_tv.pt')

# 원하는 해상도 설정
desired_width = 640
desired_height = 480

url="http://192.168.123.107:8080/shot.jpg"  # IP와 포트는 카메라 설정에 맞게 변경

while True:
    img_arr = np.array(bytearray(urllib.request.urlopen(url).read()), dtype=np.uint8)
    frame = cv2.imdecode(img_arr, -1)

    # 이미지를 원하는 해상도로 조절
    frame = cv2.resize(frame, (desired_width, desired_height))

    if frame is not None:
        results = model.track(frame, persist=True)

        if results and results[0].boxes:  # 결과와 박스가 있는지 확인
            boxes = results[0].boxes.xywh.cpu()

            annotated_frame = results[0].plot()

            for box in boxes:
                x, y, w, h = box
                top_left = (x, y)
                top_right = (x + w, y)
                bottom_left = (x, y + h)
                bottom_right = (x + w, y + h)
                print(f"Bounding Box Corners: Top-left: {top_left}, Top-right: {top_right}, Bottom-left: {bottom_left}, Bottom-right: {bottom_right}")

            cv2.imshow("YOLOv8 Tracking", annotated_frame)

            if cv2.waitKey(1) & 0xFF == ord("q"):
                break
        else:
            print("No detections")
    else:
        break

cv2.destroyAllWindows()
