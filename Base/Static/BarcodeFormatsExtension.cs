using BarcodeScanner.Mobile;
using Base.Enums;

namespace Base.Static
{

    public static class BarcodeFormatsExtension
    {
        public static bool CompareBarcodeFormatToChooseFormat(this BarcodeFormats barcodes, ScanChoose choisedFormats)
        {
            switch (choisedFormats)
            {
                case ScanChoose.Штрихкод:
                    {
                        if (barcodes == BarcodeFormats.Ean8 ||
                            barcodes == BarcodeFormats.Ean13 ||
                            barcodes == BarcodeFormats.Code39 ||
                            barcodes == BarcodeFormats.Code93 ||
                            barcodes == BarcodeFormats.Code128)
                            return true;
                        break;
                    }
                case ScanChoose.Акциз:
                    {
                        if (barcodes == BarcodeFormats.DataMatrix ||
                            barcodes == BarcodeFormats.QRCode)
                            return true;
                        break;
                    }
                default:
                    break;
            }
            return false;
        }
    }
}
