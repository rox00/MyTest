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
	//创建共享文件句柄
	HANDLE hMapFile = CreateFileMapping(INVALID_HANDLE_VALUE,//物理文件句柄 
		NULL,
		PAGE_READWRITE,//可读可写
		0,
		BUF_SIZE,
		TEXT("MySharedMemory"));//共享内存名称
	if (hMapFile == NULL) {
		cout << "创建共享内存失败!" << endl;
		return;
	}
	//映射缓存区视图，得到共享内存的指针
	LPVOID pBuf = (LPTSTR)MapViewOfFile(hMapFile,//已经创建的文件映射对象的句柄
		FILE_MAP_ALL_ACCESS,//可读可写
		0,
		0,
		BUF_SIZE);//映射视图的大小
	if (pBuf == NULL) {
		cout << "映射缓存区视图失败" << endl;
		return;
	}
}

void openSharedMemCpp() {
	//打开共享文件句柄
	HANDLE hMapFile = OpenFileMapping(FILE_MAP_ALL_ACCESS,//访问模式，可读可写
		0,
		TEXT("MySharedMemory"));//共享内存名称
	if (NULL == hMapFile) {
		DWORD err = ::GetLastError();
		cout << "打开共享文件失败" << err << endl;
		return;
	}
	//得到共享内存指针
	LPVOID pBuf = (LPTSTR)MapViewOfFile(hMapFile,
		FILE_MAP_ALL_ACCESS,
		0,
		0,
		0);
	if (pBuf == NULL) {
		cout << "映射共享内存失败" << endl;
		return;
	}

}


int main()
{
	// 创建共享内存
	HANDLE hMapFile = CreateFileMapping(
		INVALID_HANDLE_VALUE,   // 使用无效的文件句柄
		NULL,                   // 默认安全属性
		PAGE_READWRITE,         // 可读写权限
		0,                      // 共享内存大小（0表示文件大小）
		4096,                   // 共享内存名称
		TEXT("MySharedMemory")
	);
	if (hMapFile == NULL)
	{
		std::cout << "无法创建共享内存，错误代码：" << GetLastError() << std::endl;
		return 1;
	}
	// 将共享内存映射到进程的地址空间
	LPVOID pBuf = MapViewOfFile(
		hMapFile,       // 共享内存句柄
		FILE_MAP_ALL_ACCESS,    // 可读写权限
		0,
		0,
		4096);
	if (pBuf == NULL)
	{
		std::cout << "无法映射共享内存，错误代码：" << GetLastError() << std::endl;
		CloseHandle(hMapFile);
		return 1;
	}
	// 写入数据到共享内存
	strcpy_s((char*)pBuf, 4096, "Hello, shared memory!");
	// 等待用户输入
	std::cin.get();
	// 解除内存映射
	UnmapViewOfFile(pBuf);
	// 关闭共享内存句柄
	CloseHandle(hMapFile);
	return 0;
}