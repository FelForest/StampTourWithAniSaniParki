import urllib.request
import cv2
import numpy as np

# 원하는 해상도 설정
desired_width = 640
desired_height = 480

url="http://192.168.123.107:8080/shot.jpg"
while True:
    img_arr = np.array(bytearray(urllib.request.urlopen(url).read()), dtype=np.uint8)
    img = cv2.imdecode(img_arr, -1)
    
    # 이미지를 원하는 해상도로 조절
    img = cv2.resize(img, (desired_width, desired_height))
    
    cv2.imshow("IP webCAm", img)
    if cv2.waitKey(1) == ord('q'):
        break
    
cv2.destroyAllWindows()
