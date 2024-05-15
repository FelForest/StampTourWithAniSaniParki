import socket

# 서버 설정
server_ip = '192.168.131.150'
server_port = 8080
server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server_socket.bind((server_ip, server_port))
server_socket.listen(5)  # 동시에 여러 클라이언트를 받기 위해 listen을 사용

print("서버 시작, 연결 대기중...")

try:
    while True:
        # 클라이언트 연결 수락
        client_socket, addr = server_socket.accept()
        print(f"{addr} 에서 연결됨")

        # 연결된 클라이언트로부터 데이터 받기
        while True:
            data = client_socket.recv(1024)  # 데이터 수신
            if not data:
                break  # 데이터가 없으면 연결 종료
            message = data.decode('utf-8')
            print("수신된 데이터:", message)

        client_socket.close()
        print("연결 종료:", addr)

except KeyboardInterrupt:
    print("서버를 종료합니다.")
finally:
    server_socket.close()
