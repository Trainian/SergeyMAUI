using BarcodeScanner.Mobile;
using Base.Enums;
using Base.Models;
using Base.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Interfaces
{
    public interface IScanService
    {
        /// <summary>
        /// Получение информации о коде и добавление его в список
        /// </summary>
        /// <param name="barcode">Отсканированный элемент</param>
        /// <param name="scanChoose"></param>
        /// <param name="guid"></param>
        /// <param name="serverConnected">Есть ли соединение с сервером 1С</param>
        /// <param name="internetConnected"></param>
        /// <returns>Возврат измененного элемента с данными, при дублировании QR возврат 'null'</returns>
        Task<ScanElement> CreateScanElementAsync(BarcodeResult barcode, ScanChoose scanChoose, string guid, bool serverConnected, bool internetConnected);

        /// <summary>
        /// Проверяем на верный выбор формата сканирования
        /// </summary>
        /// <param name="code">Отсканированный код</param>
        /// <param name="method">Выбранные метод сканирования</param>
        /// <returns>True - выбран верный метод сканирования, False - не верный метод сканирования</returns>
        bool IsCorrectChooseScannedCode(BarcodeResult code, ScanChoose method);
    }
}
