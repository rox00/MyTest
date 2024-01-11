#include <windows.h>
#include <winnt.h>
#include <memoryapi.h>
#include <handleapi.h>
#include <errhandlingapi.h>
#include <iostream>
using namespace std;

#





void createSharedMemCpp() {
	int BUF_SIZE = 256;
	//���������ļ����
	HANDLE hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE,//�����ļ���� 
		NULL,
		PAGE_READWRITE,//�ɶ���д
		0,
		BUF_SIZE,
		TEXT("MySharedMemory"));//�����ڴ�����
	if (hMapFile == NULL) {
		cout << "���������ڴ�ʧ��!" << endl;
		return;
	}
	//ӳ�仺������ͼ���õ������ڴ��ָ��
	LPVOID pBuf = (LPTSTR)MapViewOfFile(hMapFile,//�Ѿ��������ļ�ӳ�����ľ��
		FILE_MAP_ALL_ACCESS,//�ɶ���д
		0,
		0,
		BUF_SIZE);//ӳ����ͼ�Ĵ�С
	if (pBuf == NULL) {
		cout << "ӳ�仺������ͼʧ��" << endl;
		return;
	}
}

void openSharedMemCpp() {
	//�򿪹����ļ����
	HANDLE hMapFile = OpenFileMapping(FILE_MAP_ALL_ACCESS,//����ģʽ���ɶ���д
		0,
		TEXT("MySharedMemory"));//�����ڴ�����
	if (NULL == hMapFile) {
		DWORD err = ::GetLastError();
		cout << "�򿪹����ļ�ʧ��" << err << endl;
		return;
	}
	//�õ������ڴ�ָ��
	LPVOID pBuf = (LPTSTR)MapViewOfFile(hMapFile,
		FILE_MAP_ALL_ACCESS,
		0,
		0,
		0);
	if (pBuf == NULL) {
		cout << "ӳ�乲���ڴ�ʧ��" << endl;
		return;
	}

}


int main()
{
	// ���������ڴ�
	HANDLE hMapFile = CreateFileMapping(
		INVALID_HANDLE_VALUE,   // ʹ����Ч���ļ����
		NULL,                   // Ĭ�ϰ�ȫ����
		PAGE_READWRITE,         // �ɶ�дȨ��
		0,                      // �����ڴ��С��0��ʾ�ļ���С��
		4096,                   // �����ڴ�����
		TEXT("MySharedMemory")
	);
	if (hMapFile == NULL)
	{
		std::cout << "�޷����������ڴ棬������룺" << GetLastError() << std::endl;
		return 1;
	}
	// �������ڴ�ӳ�䵽���̵ĵ�ַ�ռ�
	LPVOID pBuf = MapViewOfFile(
		hMapFile,       // �����ڴ���
		FILE_MAP_ALL_ACCESS,    // �ɶ�дȨ��
		0,
		0,
		4096);
	if (pBuf == NULL)
	{
		std::cout << "�޷�ӳ�乲���ڴ棬������룺" << GetLastError() << std::endl;
		CloseHandle(hMapFile);
		return 1;
	}
	// д�����ݵ������ڴ�
	strcpy_s((char*)pBuf, 4096, "Hello, shared memory!");
	// �ȴ��û�����
	std::cin.get();
	// ����ڴ�ӳ��
	UnmapViewOfFile(pBuf);
	// �رչ����ڴ���
	CloseHandle(hMapFile);
	return 0;
}