$(function() {
        $(".item").draggable({
                revert: true
        });
        $("#trash").droppable({
                tolerance: 'touch',
                over: function() {
                       $(this).removeClass('out').addClass('over');
                },
                out: function() {
                        $(this).removeClass('over').addClass('out');
                },
                drop: function() {
                        // var answer = confirm('Permantly delete this item?');
                        // $(this).removeClass('over').addClass('out');
                      this.hide();
                }
        });
});