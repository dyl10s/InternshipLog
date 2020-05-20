function generatePDF(id) {
    //var printDoc = new jsPDF();  
    //printDoc.fromHTML($('#' + id).get(0), 10, 10, { 'width': 180 });
    //printDoc.autoPrint();
    //printDoc.output("dataurlnewwindow");

    var printContent = document.getElementById(id);
    var WinPrint = window.open('', '', 'width=900,height=650');
    WinPrint.document.write(printContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();
    WinPrint.print();
}