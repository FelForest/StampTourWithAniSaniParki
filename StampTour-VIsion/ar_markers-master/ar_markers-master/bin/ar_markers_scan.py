#!/usr/bin/env python

import cv2
import numpy as np
import urllib.request
import socket

try:
    from ar_markers import detect_markers
except ImportError:
    raise Exception('Error: ar_markers is not installed')

# 원하는 해상도 설정
desired_width = 640
desired_height = 480

url = "http://192.168.131.86:8080/shot.jpg"

# TCP 클라이언트 설정
server_ip = '192.168.131.86'  # 서버의 IP 주소
server_port = 8080           # 서버의 포트 번호
client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client_socket.connect((server_ip, server_port))

if __name__ == '__main__':
    print('Press "q" to quit')
    
    while True:
        img_arr = np.array(bytearray(urllib.request.urlopen(url).read()), dtype=np.uint8)
        frame = cv2.imdecode(img_arr, -1)
        
        # 이미지를 원하는 해상도로 조절
        frame = cv2.resize(frame, (desired_width, desired_height))
        
        markers = detect_markers(frame)
        for marker in markers:
            marker_id_message = f"Marker ID: {marker.id}"
            print(marker_id_message)
            
            # 마커 ID 서버로 전송
            client_socket.sendall(marker_id_message.encode())

            marker.highlite_marker(frame)
        
        cv2.imshow("IP Webcam", frame)
        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    
    cv2.destroyAllWindows()
    client_socket.close()
