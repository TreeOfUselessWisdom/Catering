document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS
    if (window.AOS) {
        AOS.init({ duration: 800, once: true });
    }

    // — Back to Top Button —
    const backBtn = document.getElementById('backToTop');
    if (backBtn) {
        window.addEventListener('scroll', () => {
            backBtn.style.display = window.scrollY > 300 ? 'block' : 'none';
        });
        backBtn.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    // — Burger Menu Toggle —
    const menuToggle = document.getElementById('menuToggle');
    const overlay = document.getElementById('overlayMenu');
    const navbar = document.querySelector('.navbar');
    const bodyEl = document.body;
    const overlayLinks = document.querySelectorAll('#overlayMenu a');

    if (menuToggle && overlay && navbar && bodyEl) {
        menuToggle.addEventListener('click', () => {
            const icon = menuToggle.querySelector('i');
            const isOpening = !overlay.classList.contains('show');

            overlay.classList.toggle('show');
            overlay.classList.toggle('solid', isOpening);
            if (icon) {
                icon.classList.toggle('fa-bars', !isOpening);
                icon.classList.toggle('fa-times', isOpening);
            }

            bodyEl.classList.toggle('no-scroll', isOpening);
            navbar.classList.toggle('menu-open', isOpening);
        });

        overlayLinks.forEach(link => {
            link.addEventListener('click', () => {
                overlay.classList.remove('show', 'solid');
                bodyEl.classList.remove('no-scroll');
                navbar.classList.remove('menu-open');

                const icon = menuToggle.querySelector('i');
                if (icon) {
                    icon.classList.add('fa-bars');
                    icon.classList.remove('fa-times');
                }
            });
        });
    }

    // — Accessibility Panel Toggle —
    const accBtn = document.getElementById('accessibilityBtn');
    const accPanel = document.getElementById('accessibilityPanel');
    if (accBtn && accPanel) {
        accBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            accPanel.classList.toggle('active');
        });
        document.addEventListener('click', (e) => {
            if (!accPanel.contains(e.target) && !accBtn.contains(e.target)) {
                accPanel.classList.remove('active');
            }
        });
        accPanel.addEventListener('click', (e) => e.stopPropagation());
    }

    // — Accessibility Feature Buttons —
    let textSizeState = 0;
    document.querySelectorAll('.accessibility-option').forEach(btn => {
        btn.addEventListener('click', () => {
            const feature = btn.dataset.feature;
            switch (feature) {
                case 'textEnlarge':
                    document.body.classList.remove('text-size-1', 'text-size-2', 'text-size-3');
                    textSizeState = (textSizeState + 1) % 4;
                    if (textSizeState > 0) {
                        document.body.classList.add(`text-size-${textSizeState}`);
                    }
                    const indicator = btn.querySelector('.text-size-indicator');
                    if (indicator) {
                        indicator.querySelectorAll('span').forEach((span, i) => {
                            span.classList.toggle('active', i < textSizeState);
                        });
                    }
                    btn.classList.toggle('active', textSizeState > 0);
                    break;

                case 'hideImages':
                    document.body.classList.toggle('hide-images');
                    break;

                case 'pauseAnimations':
                    document.body.classList.toggle('no-animations');
                    break;

                case 'tooltips':
                    toggleTooltips(btn);
                    break;

                case 'highlightLinks':
                    document.body.classList.toggle('highlight-links');
                    break;
            }
        });
    });

    // — Tooltip logic —
    let tooltipEnabled = false;
    let currentTooltip = null;
    function toggleTooltips(btn) {
        const elements = document.querySelectorAll('[data-tooltip]');
        tooltipEnabled = !tooltipEnabled;

        if (tooltipEnabled) {
            currentTooltip = document.createElement('div');
            currentTooltip.className = 'custom-tooltip';
            document.body.appendChild(currentTooltip);
            elements.forEach(el => {
                el.addEventListener('mouseenter', showTooltip);
                el.addEventListener('mousemove', moveTooltip);
                el.addEventListener('mouseleave', hideTooltip);
            });
            btn.classList.add('active');
        } else {
            if (currentTooltip) {
                currentTooltip.remove();
                currentTooltip = null;
            }
            elements.forEach(el => {
                el.removeEventListener('mouseenter', showTooltip);
                el.removeEventListener('mousemove', moveTooltip);
                el.removeEventListener('mouseleave', hideTooltip);
            });
            btn.classList.remove('active');
        }
    }
    function showTooltip(e) {
        if (!currentTooltip) return;
        currentTooltip.textContent = this.dataset.tooltip;
        currentTooltip.style.display = 'block';
        moveTooltip(e);
    }
    function moveTooltip(e) {
        if (!currentTooltip) return;
        currentTooltip.style.left = (e.clientX + 15) + 'px';
        currentTooltip.style.top = (e.clientY + 15) + 'px';
    }
    function hideTooltip() {
        if (currentTooltip) {
            currentTooltip.style.display = 'none';
        }
    }

    // — Contact Form Validation —
    const form = document.getElementById('contactForm');
    if (form) {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                event.preventDefault();
                alert('Thank you! Your message has been sent.');
                form.reset();
                form.classList.remove('was-validated');
            }
            form.classList.add('was-validated');
        }, false);
    }
});
