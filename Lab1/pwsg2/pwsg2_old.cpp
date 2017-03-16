// pwsg2.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "pwsg2.h"

#define MAX_LOADSTRING 100
#define SQUARE 25
#define MAX_COUNT 30

std::set<std::pair<int, int>> checkedWins;
int countX = 30;
int countY = 30;
int mines = 2;
int goodFlagged = 0;
// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name
WCHAR smallClass[MAX_LOADSTRING];
HWND smallWindows[MAX_COUNT][MAX_COUNT];
HWND mainWindow;
HINSTANCE main_hInstance;
int main_nCmdShow;
int timer_size = 100;
std::map<HWND, int> status;
enum Mode
{
	NORMAL_GAME = 0,
	GAME_LOST = 1,
	DEBUG_MODE = 2,
	DEBUG_MODE_LOST = 3
};
int mode = NORMAL_GAME;
enum Status_Enum
{
	CLEAR_STATUS = 0,
	FLAGGED_STATUS = 1,
	MINE_STATUS = 2,
	FLAGGED_MINE_STATUS = 3,
	CLICKED_STATUS = 4,
	CLICKED_BOMB_STATUS
};
int Colours[9] = {
	0,
	RGB(255, 0, 0),
	RGB(0, 255, 0),
	RGB(0, 0, 255),
	RGB(255, 255, 0),
	RGB(0, 255, 255),
	RGB(255, 150, 30),
	RGB(130, 0, 200),
	RGB(0, 200, 100) };
int menu = 49;
int border = 8;

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
ATOM                MySmallClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
LRESULT CALLBACK    SmallProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    Customise(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);


    // Initialize global strings
    LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadStringW(hInstance, IDC_PWSG2, szWindowClass, MAX_LOADSTRING);
	LoadStringW(hInstance, IDC_SMALL, smallClass, MAX_LOADSTRING);
    MyRegisterClass(hInstance);
	MySmallClass(hInstance);
	main_hInstance = hInstance;
	main_nCmdShow = nCmdShow;
    // Perform application initialization:
    if (!InitInstance (hInstance, nCmdShow))
    {
        return FALSE;
    }

    HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_PWSG2));

    MSG msg;

    // Main message loop:
    while (GetMessage(&msg, nullptr, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }

    return (int) msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
bool reveal(int i, int j)
{
	if (checkedWins.count(std::pair<int, int>(i, j)) == 1)
		return true;
	checkedWins.insert(std::pair<int, int>(i, j));
	if (status[smallWindows[i][j]] > 10 && status[smallWindows[i][j]] < 20)
	{
		RECT rect;
		status[smallWindows[i][j]] += 20;
		GetClientRect(smallWindows[i][j], &rect);
		InvalidateRect(smallWindows[i][j], &rect, true);
		return true;
	}
	RECT rect;
	status[smallWindows[i][j]] = CLICKED_STATUS;
	if (i > 0 && ((status[smallWindows[i - 1][j]] > 10 && status[smallWindows[i - 1][j]] < 20) || status[smallWindows[i - 1][j]] == CLEAR_STATUS))
		reveal(i - 1, j);
	if (i > 0 && j > 0 && (status[smallWindows[i - 1][j - 1]] == CLEAR_STATUS || (status[smallWindows[i - 1][j - 1]] < 20 && status[smallWindows[i - 1][j - 1]] > 10)))
		reveal(i - 1, j-1);
	if (i > 0 && j < countY - 1 && (status[smallWindows[i - 1][j + 1]] == CLEAR_STATUS || (status[smallWindows[i - 1][j + 1]] > 10 && status[smallWindows[i - 1][j + 1]] < 20)))
		reveal(i - 1, j+1);
	if (j > 0 && (status[smallWindows[i][j - 1]] == CLEAR_STATUS || (status[smallWindows[i][j - 1]] > 10 && status[smallWindows[i][j - 1]] < 20)))
		reveal(i, j-1);
	if (j < countY - 1 && (status[smallWindows[i][j + 1]] == CLEAR_STATUS || (status[smallWindows[i][j + 1]] > 10 && status[smallWindows[i][j + 1]] < 20)))
		reveal(i, j+1);
	if (i < countX - 1 && j > 0 && (status[smallWindows[i + 1][j - 1]] == CLEAR_STATUS || (status[smallWindows[i + 1][j - 1]] > 10 && status[smallWindows[i + 1][j - 1]] < 20)))
		reveal(i + 1, j - 1);
	if (i < countX - 1 && (status[smallWindows[i + 1][j]] == CLEAR_STATUS || (status[smallWindows[i + 1][j]] > 10 && status[smallWindows[i + 1][j]] < 20)))
		reveal(i + 1, j);
	if (i < countX - 1 && j < countY - 1 && (status[smallWindows[i + 1][j + 1]] == CLEAR_STATUS || (status[smallWindows[i + 1][j + 1]] > 10 && status[smallWindows[i + 1][j + 1]] < 20)))
		reveal(i + 1, j + 1);
	GetClientRect(smallWindows[i][j], &rect);
	InvalidateRect(smallWindows[i][j], &rect, true);
	return true;
}
ATOM MyRegisterClass(HINSTANCE hInstance)
{
    WNDCLASSEXW wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style          = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc    = WndProc;
    wcex.cbClsExtra     = 0;
    wcex.cbWndExtra     = 0;
    wcex.hInstance      = hInstance;
    wcex.hIcon          = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PWSG2));
    wcex.hCursor        = LoadCursor(nullptr, IDC_ARROW);
    wcex.hbrBackground  = CreateSolidBrush(RGB(255, 255, 255));
    wcex.lpszMenuName   = MAKEINTRESOURCEW(IDC_PWSG2);
    wcex.lpszClassName  = szWindowClass;
    wcex.hIconSm        = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    return RegisterClassExW(&wcex);
}
ATOM MySmallClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW | WS_EX_TOPMOST;
	wcex.lpfnWndProc = SmallProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_PWSG2));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = CreateSolidBrush(RGB(0, 0, 0));
	wcex.lpszMenuName = NULL;
	wcex.lpszClassName = smallClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}
//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
bool initGame(HWND hWnd)
{
	srand((unsigned)time(NULL));
	std::set<std::pair<int,int>> rdn;
	for (int i = 0; i < mines; i++)
	{
		bool used = true;
		while (used)
		{
			used = false;
			int r1 = rand() % (countX+1);
			int r2 = rand() % (countY + 1);
			if (rdn.count(std::pair<int,int>(r1,r2)) == 1) { used = true; break; }
			if (!used)
				rdn.insert(std::pair<int,int>(r1,r2));
		}
	}
	for (int i = 0; i < countX; i++)
	{
		for (int j = 0; j < countY; j++)
		{
			smallWindows[i][j] = CreateWindowEx(NULL, smallClass, szTitle, WS_CHILD,
				SQUARE*i, SQUARE*j + timer_size, SQUARE - 1, SQUARE - 1, hWnd, nullptr, main_hInstance, nullptr);
			if (rdn.count(std::pair<int,int>(i,j)))
			{
				status[smallWindows[i][j]] = MINE_STATUS;
			}
			else
				status[smallWindows[i][j]] = CLEAR_STATUS;
			ShowWindow(smallWindows[i][j], main_nCmdShow);
			UpdateWindow(smallWindows[i][j]);
		}
	}
	for (int i = 0; i < countX; i++)
	{
		for (int j = 0; j < countY; j++)
		{
			if (status[smallWindows[i][j]] == MINE_STATUS)
				continue;
			int count = 0;
			if (i > 0 && status[smallWindows[i - 1][j]] == MINE_STATUS)
				count++;
			if (i > 0 && j > 0 && status[smallWindows[i - 1][j - 1]] == MINE_STATUS)
				count++;
			if (i > 0 && j < countY-1 && status[smallWindows[i - 1][j + 1]] == MINE_STATUS)
				count++;
			if (j > 0 && status[smallWindows[i][j - 1]] == MINE_STATUS)
				count++;
			if (j < countY - 1 && status[smallWindows[i][j + 1]] == MINE_STATUS)
				count++;
			if (i < countX - 1 && j > 0 && status[smallWindows[i + 1][j - 1]] == MINE_STATUS)
				count++;
			if (i < countX - 1 && status[smallWindows[i + 1][j]] == MINE_STATUS)
				count++;
			if (i < countX - 1 && j < countY - 1 && status[smallWindows[i + 1][j + 1]] == MINE_STATUS)
				count++;
			if (count != 0)
			{
				status[smallWindows[i][j]] = count + 10;
				if (mode == DEBUG_MODE || mode == DEBUG_MODE_LOST)
				{
					HDC wind = GetDC(smallWindows[i][j]);
					SetBkMode(wind, TRANSPARENT);
					wchar_t buffer[256];
					wsprintfW(buffer, L"%d", count);
					SetTextAlign(wind, TA_CENTER);
					SetTextColor(wind, Colours[count]);
					TextOut(wind, 12, 4, buffer, 1);
					ReleaseDC(smallWindows[i][j], wind);
					RedrawWindow(smallWindows[i][j], NULL, NULL, RDW_INVALIDATE);
				}
			}
		}
	}
	return true;
}
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
   hInst = hInstance; // Store instance handle in our global variable

   HWND hWnd = CreateWindowEx(NULL, szWindowClass, szTitle, (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
      200, 200, 100+SQUARE*countX + 2 * border-1, 100 + SQUARE*countY+menu+timer_size+border, 0, nullptr, nullptr, hInstance);
   mainWindow = hWnd;
   CreateWindowEx(NULL, smallClass, szTitle, WS_CHILD,
	   0, 0, SQUARE*countX + 2 * border - 1, timer_size, 0, nullptr, main_hInstance, nullptr);
   initGame(hWnd);
   if (!hWnd)
   {
      return FALSE;
   }

   ShowWindow(hWnd, nCmdShow);
   UpdateWindow(hWnd);
   
   return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND  - process the application menu
//  WM_PAINT    - Paint the main window
//  WM_DESTROY  - post a quit message and return
//
//
LRESULT CALLBACK SmallProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	static int curr_status = 0;
	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;
	case WM_PAINT:
	{
		PAINTSTRUCT ps;

		HDC wind = BeginPaint(hWnd, &ps);
		if(status[hWnd] == FLAGGED_STATUS || status[hWnd] == FLAGGED_MINE_STATUS || status[hWnd] >= 20 && status[hWnd] <= 30)
		{
			HDC hdc = GetDC(hWnd);
			HBITMAP bitmap = LoadBitmap(hInst, MAKEINTRESOURCE(IDB_BITMAP1));
			HDC memDC = CreateCompatibleDC(hdc);
			HBITMAP oldBitmap = (HBITMAP)SelectObject(memDC, bitmap);
			BitBlt(hdc, 0, 0, 48, 48, memDC, 0, 0, SRCCOPY);
			StretchBlt(hdc, 0, 0, SQUARE, SQUARE, memDC, 0, 0, 48, 48, SRCCOPY);
			SelectObject(memDC, oldBitmap);
			DeleteObject(bitmap);
			ReleaseDC(hWnd, hdc);
			DeleteDC(memDC);
		}
		if (mode == DEBUG_MODE || mode == DEBUG_MODE_LOST)
		{
			if (status[hWnd] >= 10 && status[hWnd] <= 20)
			{
				int count = status[hWnd] - 10;
				SetBkMode(wind, TRANSPARENT);
				status[hWnd] = count + 10;
				wchar_t buffer[256];
				wsprintfW(buffer, L"%d", count);
				SetTextColor(wind, Colours[count]);
				SetTextAlign(wind, TA_CENTER);
				TextOut(wind, 12, 4, buffer, 1);
			}
			if (status[hWnd] >= 20 && status[hWnd] <= 30)
			{
				int count = status[hWnd] - 20;
				SetBkMode(wind, TRANSPARENT);
				status[hWnd] = count + 20;
				wchar_t buffer[256];
				wsprintfW(buffer, L"%d", count);
				SetTextAlign(wind, TA_CENTER);
				SetTextColor(wind, Colours[count]);
				TextOut(wind, 12, 4, buffer, 1);
			}
		}
		if (status[hWnd] >= 30 && status[hWnd] <= 40)
		{
			int count = status[hWnd] - 30;
			SetBkMode(wind, TRANSPARENT);
			status[hWnd] = count + 30;
			wchar_t buffer[256];
			wsprintfW(buffer, L"%d", count);
			SetTextAlign(wind, TA_CENTER);
			SetTextColor(wind, Colours[count]);
			TextOut(wind, 12, 4, buffer, 1);
		}

		if (curr_status >= 30 && curr_status <= 40)
		{
			int count = curr_status - 20;
			SetBkMode(wind, TRANSPARENT);
			status[hWnd] = count + 10;
			wchar_t buffer[256];
			wsprintfW(buffer, L"%d", count);
			SetTextAlign(wind, TA_CENTER);
			SetTextColor(wind, Colours[count]);
			TextOut(wind, 12, 4, buffer, 1);
		}
		SelectObject(wind, GetStockObject(DC_BRUSH));
		SetDCBrushColor(wind, RGB(0, 0, 0));
		if (status[hWnd] == CLICKED_BOMB_STATUS)
			Ellipse(wind, 5, 5, SQUARE - 5, SQUARE - 5);
		if (mode == DEBUG_MODE || mode == DEBUG_MODE_LOST)
		{
			switch (status[hWnd])
			{
			case MINE_STATUS:
				Ellipse(wind, 5, 5, SQUARE - 5, SQUARE - 5);
				break;
			case FLAGGED_MINE_STATUS:
				Ellipse(wind, 5, 5, SQUARE - 5, SQUARE - 5);
				break;
			default:
			{
				if (curr_status >= 10 && curr_status <= 20)
				{
					int count = curr_status - 10;
					HDC wind = GetDC(hWnd);
					SetBkMode(wind, TRANSPARENT);
					status[hWnd] = count + 20;
					wchar_t buffer[256];
					wsprintfW(buffer, L"%d", count);
					SetTextColor(wind, Colours[count]);
					SetTextAlign(wind, TA_CENTER);
					TextOut(wind, 12, 4, buffer, 1);
					ReleaseDC(hWnd, wind);
				}
				if (curr_status >= 20 && curr_status <= 30)
				{
					int count = curr_status - 20;
					HDC wind = GetDC(hWnd);
					SetBkMode(wind, TRANSPARENT);
					status[hWnd] = count + 10;
					wchar_t buffer[256];
					wsprintfW(buffer, L"%d", count);
					SetTextAlign(wind, TA_CENTER);
					SetTextColor(wind, Colours[count]);
					TextOut(wind, 12, 4, buffer, 1);
					ReleaseDC(hWnd, wind);
				}
			}
			break;
			}
		}

		EndPaint(hWnd, &ps);
	}
	break;
	case WM_LBUTTONDOWN:
	{
		if (mode == GAME_LOST || mode == DEBUG_MODE_LOST)
			return 0;
		if (status[hWnd] == MINE_STATUS)
		{
			LPCTSTR Caption = L"Mine";
			if (mode == NORMAL_GAME)
				mode = GAME_LOST;
			else
				mode = DEBUG_MODE_LOST;
			MessageBox(NULL,
				L"Boom!",
				Caption,
				MB_OK | MB_ICONERROR | MB_SYSTEMMODAL);
			status[hWnd] = CLICKED_BOMB_STATUS;
			RECT rect;
			GetClientRect(hWnd, &rect);
			InvalidateRect(hWnd, &rect, true);
		}
		else if (status[hWnd] != FLAGGED_MINE_STATUS && status[hWnd] != FLAGGED_STATUS && status[hWnd] < 20)
		{
			int i1, j1;
			bool cont = true;
			for (int i = 0; i < countX && cont; i++)
			{
				for (int j = 0; j < countY && cont; j++)
				{
					if (hWnd == smallWindows[i][j])
					{
						i1 = i;
						j1 = j;
						cont = false;
					}
				}
			}
			reveal(i1, j1);
			checkedWins.clear();
		}
	}
	break;
	case WM_RBUTTONDOWN:
	{
		if (mode == GAME_LOST || mode == DEBUG_MODE_LOST)
			return 0;
		bool reload = true;
		switch (status[hWnd])
		{
		case CLICKED_STATUS:
			reload = false;
			break;
		case CLEAR_STATUS:
			status[hWnd] = FLAGGED_STATUS;
			break;
		case MINE_STATUS:
			goodFlagged++;
			if (goodFlagged == mines)
			{
				mode = GAME_LOST;
				//display win dialog
			}
			status[hWnd] = FLAGGED_MINE_STATUS;
			break;
		case FLAGGED_MINE_STATUS:
			status[hWnd] = MINE_STATUS;
			break;
		case FLAGGED_STATUS:
			status[hWnd] = CLEAR_STATUS;
			break;
		default:
			curr_status = status[hWnd];
			break;
		}
		if (status[hWnd] > 30)
			reload = false;
		if (reload)
		{
			RECT rect;
			GetClientRect(hWnd, &rect);
			InvalidateRect(hWnd, &rect, true);
		}
	}
	break;
	case WM_ERASEBKGND:
	{
		HBRUSH colour;
		HDC wind = GetDC(hWnd);
		switch (status[hWnd])
		{
		case CLICKED_STATUS:
			colour = CreateSolidBrush(RGB(255, 255, 255));
			break;
		case CLEAR_STATUS:
			colour = CreateSolidBrush(RGB(170, 170, 170));
			break;
		case MINE_STATUS:
			colour = CreateSolidBrush(RGB(170, 170, 170));
			break;
		case CLICKED_BOMB_STATUS:
			colour = CreateSolidBrush(RGB(255, 255, 255));
			break;
		default:
			{
				colour = CreateSolidBrush(RGB(170, 170, 170));
				if (curr_status >= 10 && curr_status <= 20)
				{
					colour = CreateSolidBrush(RGB(255, 0, 0));
				}
				if (curr_status >= 20 && curr_status <= 30)
				{
					colour = CreateSolidBrush(RGB(170, 170, 170));
				}
				if (status[hWnd] >= 30 && status[hWnd] <= 40)
				{
					colour = CreateSolidBrush(RGB(255, 255, 255));
				}
			}
			break;
		}

		RECT rect;
		GetClientRect(hWnd, &rect);
		FillRect(GetDC(hWnd), &rect, colour);


		curr_status = 0;
		return 1;
	}
	break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
		break;
	}
	return 0;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_COMMAND:
        {
            int wmId = LOWORD(wParam);
            // Parse the menu selections:
            switch (wmId)
            {
            case IDM_ABOUT:
                DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTME), hWnd, About);
                break;
			case IDD_CUSTOMISE:
			{
				DialogBox(hInst, MAKEINTRESOURCE(IDD_CUSTOMISE), hWnd, Customise);
			}
				break;
            case IDM_EXIT:
                DestroyWindow(hWnd);
                break;
			case IDM_DEBUG:
				if (mode == DEBUG_MODE_LOST)
				{
					mode = GAME_LOST;
					for (int i = 0; i < countX; i++)
					{
						for (int j = 0; j < countY; j++)
						{
							RECT rect;
							GetClientRect(smallWindows[i][j], &rect);
							InvalidateRect(smallWindows[i][j], &rect, false);
						}
					}
				}
				else if (mode == DEBUG_MODE)
				{
					mode = NORMAL_GAME;
					for (int i = 0; i < countX; i++)
					{
						for (int j = 0; j < countY; j++)
						{
							RECT rect;
							GetClientRect(smallWindows[i][j], &rect);
							InvalidateRect(smallWindows[i][j], &rect, false);
						}
					}
				}
				else if (mode == NORMAL_GAME)
				{
					mode = DEBUG_MODE;
					for (int i = 0; i < countX; i++)
					{
						for (int j = 0; j < countY; j++)
						{
							RECT rect;
							GetClientRect(smallWindows[i][j], &rect);
							InvalidateRect(smallWindows[i][j], &rect, true);
						}
					}
				}
				else if (mode == GAME_LOST)
				{
					mode = DEBUG_MODE_LOST;
					for (int i = 0; i < countX; i++)
					{
						for (int j = 0; j < countY; j++)
						{
							RECT rect;
							GetClientRect(smallWindows[i][j], &rect);
							InvalidateRect(smallWindows[i][j], &rect, true);
						}
					}
				}
				break;
			case IDM_NEW:
			{
				/*wchar_t buf[256];
				FormatMessageW(FORMAT_MESSAGE_FROM_SYSTEM, NULL, GetLastError(),
					MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), buf, 256, NULL);
				OutputDebugStringW(buf);*/
				if (mode == DEBUG_MODE_LOST || mode == DEBUG_MODE)
					mode = DEBUG_MODE;
				else
					mode = NORMAL_GAME;
				for (int i = 0; i < countX; i++)
				{
					for (int j = 0; j < countY; j++)
					{
						DestroyWindow(smallWindows[i][j]);
					}
				}
				initGame(hWnd);
			}
				break;
            default:
                return DefWindowProc(hWnd, message, wParam, lParam);
            }
        }
        break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
		break;
    }
    return 0;
}
INT_PTR CALLBACK Customise(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
	{
		wchar_t buffer[256];
		wsprintfW(buffer, L"%d", countY);
		HWND hCtrl = GetDlgItem(hDlg, IDC_EDIT1);
		SetWindowText(hCtrl, buffer);
		wsprintfW(buffer, L"%d", countX);
		hCtrl = GetDlgItem(hDlg, IDC_EDIT2);
		SetWindowText(hCtrl, buffer);
		wsprintfW(buffer, L"%d", mines);
		hCtrl = GetDlgItem(hDlg, IDC_EDIT3);
		SetWindowText(hCtrl, buffer);
		return (INT_PTR)TRUE;
	}
	break;
	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			if (LOWORD(wParam) == IDOK)
			{
				wchar_t buf[256];
				GetDlgItemText(hDlg, IDC_EDIT1, buf, 80);
				int ncountY = std::stoi(buf);
				GetDlgItemText(hDlg, IDC_EDIT2, buf, 80);
				int ncountX = std::stoi(buf);
				GetDlgItemText(hDlg, IDC_EDIT3, buf, 80);
				mines = std::stoi(buf);
				if (ncountX*ncountY <= mines)
					mines = ncountX*ncountY;
				EndDialog(hDlg, LOWORD(wParam));
				for (int i = 0; i < countX; i++)
				{
					for (int j = 0; j < countY; j++)
					{
						DestroyWindow(smallWindows[i][j]);
					}
				}
				countY = ncountY;
				countX = ncountX;
				if (countY >= 30)
					countY = 30;
				if (countX >= 30)
					countX = 30;
				RECT z;
				GetWindowRect(mainWindow, &z);
				SetWindowPos(mainWindow, HWND_TOP, z.left, z.top, SQUARE*countX + 2 * border - 1, SQUARE*countY + menu + timer_size + border, SWP_SHOWWINDOW);
				initGame(mainWindow);
			}
			else
				EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}
